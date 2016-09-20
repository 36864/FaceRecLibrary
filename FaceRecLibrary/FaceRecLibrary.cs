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
        private IMetadataHandler metadataHandler;

        //[ID][IdentityInfo]
        private Dictionary<string, IdentityInfo> identities;

        /// <summary>
        /// Currently loaded identities.
        /// Key: Unique ID string
        /// Value: IdentityInfo object
        /// </summary>
        public Dictionary<string, IdentityInfo> Identities { get; }

        /// <summary>
        /// FaceDetector class currently in use.
        /// Should not be used directly, but access is provided for extensibility
        /// </summary>
        public CombinedClassifierFaceDetector FaceDetector { get; }

        /// <summary>
        /// FaceRecognizer class currently in use.
        /// Should not be used directly, but access is provided for extensibility
        /// </summary>
        public LBPHFaceRecognizer FaceRecognizer { get; }



        private bool initialized = false;

        /// <summary>
        /// Initialize this library instance.
        /// </summary>
        /// <param name="classifierConfigFile">Path to the classifier configuration file.</param>
        /// <param name="recognizerSaveFile">Path to the recognizer's save file.</param>
        /// <param name="identitySaveFile">Path to the identity save file</param>
        public void Initialize(string classifierConfigFile = null, string recognizerSaveFile = null, string identitySaveFile = null, IMetadataHandler metadataHandler = null)
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
            if (metadataHandler != null)
                this.metadataHandler = metadataHandler;
            else
                this.metadataHandler = new XMPMetadataHandler();
            initialized = true;
        }

        /// <summary>
        /// Run automated face detection followed by face recognition.
        /// </summary>
        /// <param name="image">Image to process</param>
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

        /// <summary>
        /// Saves image detection metadata.
        /// </summary>
        /// <param name="images">Image of which to save metadata</param>
        /// <param name="saveParams">Parameters needed to save metadata (i.e: file path, connection strings)</param>
        public void SaveMetadata(ImageInfo image, object saveParams)
        {
            if (!initialized)
                throw new Exception("Initialize the class first by calling Init()");
            metadataHandler.Save(image, saveParams);
        }

        /// <summary>
        /// Load image metadata to the provided ImageInfo.
        /// </summary>
        /// <param name="image">Image of which to load metadata</param>
        /// <param name="loadParams">Parameters needed to load metadata (i.e: file path, connection string)</param>
        public void LoadMetadata(ImageInfo image, object loadParams)
        {
            if (!initialized)
                throw new Exception("Initialize the class first by calling Init()");
            metadataHandler.Load(image, loadParams);
        }

        /// <summary>
        /// Run face detection on multiple images.
        /// CPU-intensive, should be called from a worker thread.
        /// </summary>
        /// <param name="images">An array of ImageInfo files to be processed</param>
        /// <returns>The number of new detections for each image</returns>
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

        /// <summary>
        /// Run face recognition on multiple images.
        /// CPU-intensive, should be called from a worker thread.
        /// </summary>
        /// <param name="images">An array of ImageInfo instances to be processed</param>
        /// <returns>The number of positive identifications for each image</returns>
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


        /// <summary>
        /// Run face detection on a single image.
        /// CPU-intensive, should be called from a worker thread.
        /// </summary>
        /// <param name="image">The ImageInfo to process</param>
        /// <returns>The number of new detections</returns>
        public int DetectFaces(ImageInfo image)
        {
            if (!initialized)
                throw new Exception("Initialize the class first by calling Init()");
            return faceDetector.DetectFaces(image);
        }


        /// <summary>
        /// Run face recognizer on a single image.
        /// CPU-intensive, should be called from a worker thread.
        /// </summary>
        /// <param name="image">The ImageInfo to process</param>
        /// <returns>The number of recognitions</returns>
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
