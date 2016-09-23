using FaceRecLibrary.Types;
using FaceRecLibrary.Utilities;
using OpenCvSharp.CPlusPlus;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FaceRecLibrary
{
    public class CombinedClassifierFaceDetector
    {
        ClassifierList cList;

        bool useEyeDetection = true;
        
        public CombinedClassifierFaceDetector()
        {
            cList = new ClassifierList();
        }

        public CombinedClassifierFaceDetector(ClassifierList clist)
        {
            this.cList = clist;
        }

        public CombinedClassifierFaceDetector(string configFile)
        {
            cList = Util.LoadXmlConfigFile(configFile);
        }

        public void AddClassifier(ClassifierInfo cInfo)
        {
            if(cInfo is FaceClassifier)
            {
                cList.FaceClassifiers.Add((FaceClassifier) cInfo);
            }
            else if (cInfo is EyeClassifier)
            {
                cList.EyeClassifiers.Add((EyeClassifier) cInfo);
            }
        }

        public int DetectFaces(ImageInfo imgInfo)
        {
            FaceClassifier[] faceClassifiers = cList.FaceClassifiers.ToArray();
            EyeClassifier[] eyeClassifier = cList.EyeClassifiers.ToArray();
            Rect[][] detectionAreas = new Rect[faceClassifiers.Length][];
            int existingDetections = imgInfo.Detections?.Count ?? 0;

            Parallel.For(0, faceClassifiers.Length, (i) =>
            {
                detectionAreas[i] = FaceDetect.RunDetection(imgInfo, faceClassifiers[i]);
            });

            for (int i = 0; i < detectionAreas.Length; i++ )
            {
                imgInfo.AddDetections(Util.CvtRectToRectangle(detectionAreas[i]), faceClassifiers[i].Confidence);
            }

            //Merge and prune detections
            Util.MergeDuplicates(imgInfo.Detections);

            ;
            if (useEyeDetection)
            {
                //Further pruning through eye detection
                FaceDetect.DetectEyes(imgInfo, eyeClassifier);
                
            }            
            return imgInfo.Detections.Count - existingDetections;
        }
    }
}
