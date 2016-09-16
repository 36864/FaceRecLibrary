using FaceRecLibrary.Types;
using FaceRecLibrary.Utilities;
using SE.Halligang.CsXmpToolkit.Schemas;
using SE.Halligang.CsXmpToolkit.Schemas.Schemas;
using SE.Halligang.CsXmpToolkit.Schemas.ValueTypes;
using System;
using System.Collections.Generic;

namespace FaceRecLibrary
{
    public class FaceRecLibrary
    {
        private CombinedClassifierFaceDetector faceDetector;
        private LBPHFaceRecognizer faceRecognizer;

        //[ID][IdentityInfo]
        private Dictionary<string, IdentityInfo> identities;

        public Dictionary<string, IdentityInfo> Identities { get; }
        public CombinedClassifierFaceDetector FaceDetector { get; }
        public LBPHFaceRecognizer FaceRecognizer { get; }



        private bool initialized = false;

        public void init(string classifierConfigFile = null, string recognizerSaveFile = null, string identitySaveFile = null)
        {
            if (classifierConfigFile != null)
                faceDetector = new CombinedClassifierFaceDetector(classifierConfigFile);
            else
                faceDetector = new CombinedClassifierFaceDetector();
            if (recognizerSaveFile != null)
                faceRecognizer = new LBPHFaceRecognizer(recognizerSaveFile);
            else
                faceRecognizer = new LBPHFaceRecognizer();
            if (identitySaveFile != null)
                identities = (Dictionary<string, IdentityInfo>) Util.LoadFromXml(typeof(IdentityInfo), identitySaveFile);
            else
                identities = new Dictionary<string, IdentityInfo>();
            initialized = true;
        }

        /// <summary>
        /// Run automated face detection followed by face recognition.
        /// </summary>
        /// <param name="image"></param>
        public void DetectAndRecognize(ImageInfo image)
        {
            if (!initialized)
                throw new Exception("Initialize the class first by calling Init()");
            faceDetector.DetectFaces(image);
            try {
                faceRecognizer.Match(image);
            }
            catch (Exception) { }
        }

        public void SaveRecognizerToFile(string fileName)
        {
            if (!initialized)
                throw new Exception("Initialize the class first by calling Init()");
            faceRecognizer.Save(fileName);
        }

        public void LoadRecognizerFromFile(string fileName)
        {
            if (!initialized)
                throw new Exception("Initialize the class first by calling Init()");
            faceRecognizer.Load(fileName);
        }

        public void LoadDetectorConfigFile(string fileName)
        {
            if (!initialized)
                throw new Exception("Initialize the class first by calling Init()");
            faceDetector = new CombinedClassifierFaceDetector(fileName);
        }

        public void AddManualDetections(ImageInfo image, DetectionInfo detections)
        {
            if (!initialized)
                throw new Exception("Initialize the class first by calling Init()");
            image.DetectionInfo.Detections.AddRange(detections.Detections);
            image.DetectionInfo = Util.MergeDuplicates(new DetectionInfo[] { image.DetectionInfo, detections });
            faceRecognizer.Match(image);
            faceRecognizer.UpdateRecognizer(image);
        }

        public void SaveMetadata(ImageInfo image)
        {
            if (!initialized)
                throw new Exception("Initialize the class first by calling Init()");
            Xmp x = Xmp.FromFile(image.OriginalPath, XmpFileMode.ReadWrite);

            FaceRegionInfo fri = new FaceRegionInfo(x);
            fri.RegionList.Clear();
            fri.AppliedToDimensions.SetDimensions(image.Width, image.Height, "pixel");
            Util.MergeDuplicates(image.DetectionInfo);
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
            if (!initialized)
                throw new Exception("Initialize the class first by calling Init()");
            Xmp x = Xmp.FromFile(image.OriginalPath, XmpFileMode.ReadWrite);
            FaceRegionInfo fri = new FaceRegionInfo(x);
            foreach(FaceRegionStruct frs in fri.RegionList)
            {
                Detection d = new Detection();
                d.Area = new System.Drawing.Rectangle((int)frs.Area.X, (int)frs.Area.Y, (int)frs.Area.Width, (int)frs.Area.Height);
                d.Identity = new IdentityInfo();
                d.Identity.Name = frs.Name;
                if(image.DetectionInfo?.Detections == null || !image.DetectionInfo.Detections.Contains(d))
                    image.AddDetection(d);
            }
            x.Dispose();
        }

        public int[] DetectFaces(ImageInfo[] images)
        {
            if (!initialized)
                throw new Exception("Initialize the class first by calling Init()");
            int[] detectionCount = new int[images.Length];
            for(int i =0; i < images.Length; ++i)
            {
                detectionCount[i] = DetectFaces(images[i]);
            }
            return detectionCount;
        }

        public int[] RecognizeFaces(ImageInfo[] images)
        {
            if (!initialized)
                throw new Exception("Initialize the class first by calling Init()");
            int[] recognitionCount = new int[images.Length];
            for (int i = 0; i < images.Length; ++i)
            {
                recognitionCount[i] = RecognizeFaces(images[i]);
            }
            return recognitionCount;
        }

        public int DetectFaces(ImageInfo image)
        {
            if (!initialized)
                throw new Exception("Initialize the class first by calling Init()");
            return faceDetector.DetectFaces(image);
        }

        public int RecognizeFaces(ImageInfo image)
        {
            if (!initialized)
                throw new Exception("Initialize the class first by calling Init()");
            int numRecognized = faceRecognizer.Match(image);
            if(numRecognized > 0)
            {
                foreach(Detection d in image.DetectionInfo.Detections)
                {
                    if (d.Identity?._ID != null &&  !identities.ContainsKey(d.Identity._ID))
                        identities.Add(d.Identity._ID, d.Identity);
                }
            }
            return numRecognized;
        }
    }
}
