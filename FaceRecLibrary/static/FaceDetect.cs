using OpenCvSharp.CPlusPlus;
using FaceRecLibrary.Utilities;
using System;
using System.Threading.Tasks;
using FaceRecLibrary.Types;
namespace FaceRecLibrary
{
    public class FaceDetect
    {
        const double DEFAULT_SCALE = 1.08;
        const int DEFAULT_MIN_NEIGHBORS = 4;
        private static double CONFIDENCE_THRESHOLD = 0.95;


        /// <summary>
        /// Runs facial detection on the specified image using the specified classifier.        
        /// </summary>
        /// <param name="img"></param>
        /// <param name="classifier_paths"></param>
        /// <param name="scale_factor"></param>
        /// <param name="min_neighbors"></param>
        /// <returns>Detected face positions as rectangles, by classifier (return_value[0][1] is the second face detected by the first classifier)</returns>
        public static Rect[] RunDetection(ImageInfo imgInfo, FaceClassifier faceClassifier)
        {
            double img_scale;
            using (Mat img = Util.LoadImageForDetection(imgInfo, faceClassifier, out img_scale))
            {
                //Load classifier from classifier file (.xml)
                using (CascadeClassifier classifier = new CascadeClassifier(faceClassifier.FullName))
                {
                    return Util.ScaleRects(classifier.DetectMultiScale(img, faceClassifier.Scale, faceClassifier.MinNeighbors, OpenCvSharp.HaarDetectionType.DoCannyPruning), 1 / img_scale);
                }
            }
        }

        /// <summary>
        /// Run face detection on the specified image using all classifiers in cList
        /// </summary>
        /// <param name="imgInfo">Image to process</param>
        /// <param name="cList">List of classifiers</param>
        /// <returns></returns>
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
            DetectionInfo mergedDetections = Util.MergeDuplicates(dInfo);

            //Further pruning through eye detection
            DetectionInfo finalResult = DetectEyes(imgInfo, eyeClassifier, mergedDetections);

            return finalResult;
        }

  


        /// <summary>
        /// Run Eye detection using the specified EyeClassifiers on the facial areas detected for an image.
        /// </summary>
        /// <param name="imgInfo"></param>
        /// <param name="cInfo"></param>
        /// <param name="dInfo"></param>
        /// <returns></returns>
         public static DetectionInfo DetectEyes(ImageInfo imgInfo, EyeClassifier[] cInfo, DetectionInfo dInfo)
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

        /// <summary>
        /// Run Eye detection on a single detection area
        /// </summary>
        /// <param name="imgInfo"></param>
        /// <param name="detection"></param>
        /// <param name="classifiers"></param>
        /// <returns></returns>
        private static bool RunEyeDetection(ImageInfo imgInfo, Detection detection, EyeClassifier[] classifiers)
        {
            double out_scale;
            Mat img = Util.LoadImageForDetection(imgInfo, null, out out_scale);
            
            foreach (EyeClassifier cInfo in classifiers)
            {
                using (CascadeClassifier classifier = new CascadeClassifier(cInfo.FullName))
                    if (classifier.DetectMultiScale(img.SubMat(Util.CvtRectangletoRect(detection.Area)), cInfo.Scale, cInfo.MinNeighbors).Length == 2) // != 2 eyes probably means false positive
                        return true;
            }
            return false;
        }

        /// <summary>
        /// Run detection with a single classifier
        /// </summary>
        /// <param name="img"></param>
        /// <param name="classifier"></param>
        /// <returns></returns>
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
