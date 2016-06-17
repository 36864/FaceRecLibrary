using OpenCvSharp.CPlusPlus;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;

namespace FaceRecLibrary
{
    public class FaceDetect
    {
        const double DEFAULT_SCALE = 1.08;
        const int DEFAULT_MIN_NEIGHBORS = 4;
        private static double CONFIDENCE_THRESHOLD = 0.96;


        /// <summary>
        /// Runs facial detection on the specified image using the specified classifier.        
        /// </summary>
        /// <param name="img"></param>
        /// <param name="classifier_paths"></param>
        /// <param name="scale_factor"></param>
        /// <param name="min_neighbors"></param>
        /// <returns>Detected face positions as rectangles, by classifier (return_value[0][1] is the second face detected by the first classifier)</returns>
        private static Rect[] RunDetection(ImageInfo imgInfo, FaceClassifier faceClassifier)
        {
            double img_scale;
            using (Mat img = Util.LoadImageForDetection(imgInfo, faceClassifier, out img_scale))
            {
                //Load classifier from classifier file (.xml)
                using (CascadeClassifier classifier = new CascadeClassifier(faceClassifier.FullName))
                {
                    return Util.CvtRects(classifier.DetectMultiScale(img, faceClassifier.Scale, faceClassifier.MinNeighbors, OpenCvSharp.HaarDetectionType.DoCannyPruning), 1 / img_scale);
                }
            }

        }

        public static DetectionInfo RunDetection(ImageInfo imgInfo, ClassifierList cList)
        {
            FaceClassifier[] faceClassifiers = cList.FaceClassifiers.ToArray();
            EyeClassifier[] eyeClassifier = cList.EyeClassifiers.ToArray();

            DetectionInfo[] dInfo = new DetectionInfo[faceClassifiers.Length];

            Parallel.For(0, faceClassifiers.Length, (i) =>
            {
                //Run classifier
                       dInfo[i] = new DetectionInfo(Util.CvtRectToRectangle(RunDetection(imgInfo, faceClassifiers[i])), faceClassifiers[i].Confidence);
            });

            //Merge and prune detections
            DetectionInfo mergedDetections = MergeDuplicates(dInfo, faceClassifiers);

            //Further pruning through eye detection
            DetectionInfo finalResult = DetectEyes(imgInfo, eyeClassifier, mergedDetections);

            return finalResult;
        }

        private static DetectionInfo MergeDuplicates(DetectionInfo[] detections, FaceClassifier[] classifiers)
        {
            if (detections.Length < 1) return null;
            DetectionInfo retVal = new DetectionInfo();

            //flatten detections
            foreach (DetectionInfo dInfo in detections)
            {
                retVal.Detections.AddRange(dInfo.Detections);
            }

            //sort detections
            retVal.Detections.Sort((a, b) =>
            {
                if (a.Area.Location.Equals(b.Area.Location))
                    return 0;
                if (a.Area.Top < b.Area.Top || (a.Area.Top == b.Area.Top && a.Area.Left < b.Area.Left))
                    return 1;
                else
                    return -1;
            });

            //merge duplicates
            MergeDuplicates(retVal);


            //second pass is needed for some cases due to bi-dimensional sorting
            //sort detections
            retVal.Detections.Sort((a, b) =>
            {
                if (a.Area.Location.Equals(b.Area.Location))
                    return 0;
                if (a.Area.Left < b.Area.Left || (a.Area.Left == b.Area.Left && a.Area.Top < b.Area.Top))
                    return 1;
                else
                    return -1;
            });
            MergeDuplicates(retVal);

            return retVal;
        }

        private static void MergeDuplicates(DetectionInfo retVal)
        {
            //merge duplicates
            int i = 0;
            while (i + 1 < retVal.Detections.Count)
            {
                if (retVal.Detections[i].Area.IntersectsWith(retVal.Detections[i + 1].Area) || retVal.Detections[i].Area.Contains(retVal.Detections[i + 1].Area) || retVal.Detections[i + 1].Area.Contains(retVal.Detections[i].Area))
                {
                    if (retVal.Detections[i].Confidence > retVal.Detections[i + 1].Confidence)
                    {
                        retVal.Detections.RemoveAt(i + 1);
                    }
                    else
                    {
                        retVal.Detections.RemoveAt(i);
                    }
                }
                else ++i;
            }
        }

        private static DetectionInfo DetectEyes(ImageInfo imgInfo, EyeClassifier[] cInfo, DetectionInfo dInfo)
        {
            if (cInfo == null || cInfo.Length == 0) return dInfo;
            int i = 0;
            while (i < dInfo.Detections.Count)
            {
                if (dInfo.Detections[i].Confidence < CONFIDENCE_THRESHOLD && !RunEyeDetection(imgInfo, dInfo.Detections[i], cInfo))
                    dInfo.Detections.RemoveAt(i);
                else
                    ++i;
            }
            return dInfo;
        }

        private static bool RunEyeDetection(ImageInfo imgInfo, DetectionInfo.Detection detection, EyeClassifier[] classifiers)
        {
            double out_scale;
            Mat img = Util.LoadImageForDetection(imgInfo, null, out out_scale);
            
            foreach (EyeClassifier cInfo in classifiers)
            {
                using (CascadeClassifier classifier = new CascadeClassifier(cInfo.FullName))
                    if (classifier.DetectMultiScale(img.SubMat(Util.CvtRectangletoRect(detection.Area)), cInfo.Scale, cInfo.MinNeighbors).Length > 0)
                        return true;
            }
            return false;
        }

        public static Rect[] RunDetection(Mat img, ClassifierInfo classifier)
        {
            if (classifier.Scale < 1)
                classifier.Scale = DEFAULT_SCALE;
            if (classifier.MinNeighbors < 1)
                classifier.MinNeighbors = DEFAULT_MIN_NEIGHBORS;
            using (CascadeClassifier loadedClassifier = new CascadeClassifier(classifier.FullName))
            {
                return loadedClassifier.DetectMultiScale(img, classifier.Scale, classifier.MinNeighbors);
            }
        }
    }
}
