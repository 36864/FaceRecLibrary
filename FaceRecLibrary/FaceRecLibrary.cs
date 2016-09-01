using FaceRecLibrary.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SE.Halligang.CsXmpToolkit.Schemas.Schemas;
using SE.Halligang.CsXmpToolkit.Schemas;
using SE.Halligang.CsXmpToolkit.Schemas.ValueTypes;
using SE.Halligang.CsXmpToolkit;
using System.Windows.Media.Imaging;
using System.IO;
using System.Windows.Media;

namespace FaceRecLibrary
{
    public class FaceRecLibrary
    {
        private CombinedClassifierFaceDetector faceDetector;
        private LBPHFaceRecognizer faceRecognizer;

        public CombinedClassifierFaceDetector FaceDetector { get; }
        public LBPHFaceRecognizer FaceRecognizer { get; }


        private bool initialized = false;

        public void init(string classifierConfigFile = null, string recognizerSaveFile = null)
        {
            if (classifierConfigFile != null)
                faceDetector = new CombinedClassifierFaceDetector(classifierConfigFile);
            else
                faceDetector = new CombinedClassifierFaceDetector();
            if (recognizerSaveFile != null)
                faceRecognizer = new LBPHFaceRecognizer(recognizerSaveFile);
            else
                faceRecognizer = new LBPHFaceRecognizer();

            initialized = true;
        }

        /// <summary>
        /// Run automated face detection followed by face recognition.
        /// </summary>
        /// <param name="image"></param>
        public void DetectAndRecognize(ImageInfo image)
        {
            faceDetector.DetectFaces(image);
            faceRecognizer.Match(image);
        }

        public void SaveRecognizerToFile(string fileName)
        {
            faceRecognizer.Save(fileName);
        }

        public void LoadRecognizerFromFile(string fileName)
        {
            faceRecognizer.Load(fileName);
        }

        public void LoadDetectorConfigFile(string fileName)
        {
            faceDetector = new CombinedClassifierFaceDetector(fileName);
        }

        public void AddManualDetections(ImageInfo image, DetectionInfo detections)
        {
            image.DetectionInfo.Detections.AddRange(detections.Detections);
            image.DetectionInfo = FaceDetect.MergeDuplicates(new DetectionInfo[] { image.DetectionInfo, detections });
            faceRecognizer.Match(image);
            faceRecognizer.UpdateRecognizer(image);
        }

        public void SaveMetadata(ImageInfo image)
        {
            Xmp x = Xmp.FromFile(image.OriginalPath, XmpFileMode.ReadWrite);

            FaceRegionInfo fri = new FaceRegionInfo(x);
            fri.AppliedToDimensions.SetDimensions(image.Width, image.Height, "pixel");

            foreach (Detection d in image.DetectionInfo.Detections)
            {

                FaceArea fArea = new FaceArea(x.XmpCore, FaceRegionInfo.Namespace, "Area");

                FaceRegionStruct frs = new FaceRegionStruct(fArea, d.Identity?.Name, null, fri);
                fArea.SetValues(UnitType.Pixel, AreaType.Rectangle, d.Area.X, d.Area.Y, d.Area.Width, d.Area.Height, 0);

                fri.RegionList.Add(frs);
            }
            x.Save();
            x.Dump();
            x.Dispose();
        }

        public void LoadMetadata(ImageInfo image)
        {
            Xmp x = Xmp.FromFile(image.OriginalPath, XmpFileMode.ReadWrite);
            FaceRegionInfo fri = new FaceRegionInfo(x);
            foreach(FaceRegionStruct frs in fri.RegionList)
            {
                Detection d = new Detection();
                d.Area = new System.Drawing.Rectangle((int)frs.Area.X, (int)frs.Area.Y, (int)frs.Area.Width, (int)frs.Area.Height);
                d.Identity = new IdentityInfo();
                d.Identity.Name = frs.Name;
                image.AddDetection(d);
            }
            x.Dispose();
        }

        public int[] DetectFaces(ImageInfo[] images)
        {
            int[] detectionCount = new int[images.Length];
            for(int i =0; i < images.Length; ++i)
            {
                detectionCount[i] = DetectFaces(images[i]);
            }
            return detectionCount;
        }

        public int[] RecognizeFaces(ImageInfo[] images)
        {
            int[] recognitionCount = new int[images.Length];
            for (int i = 0; i < images.Length; ++i)
            {
                recognitionCount[i] = RecognizeFaces(images[i]);
            }
            return recognitionCount;
        }

        public int DetectFaces(ImageInfo image)
        {
            return faceDetector.DetectFaces(image);
        }

        public int RecognizeFaces(ImageInfo image)
        {
            return faceRecognizer.Match(image);
        }
    }
}
