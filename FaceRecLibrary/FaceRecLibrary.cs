using FaceRecLibrary.Types;
using FaceRecLibrary.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
namespace FaceRecLibrary
{
    public class FaceRecLibrary
    {
        private CombinedClassifierFaceDetector faceDetector;
        private LBPHFaceRecognizer faceRecognizer;
        private IMetadataHandler metadataHandler;

        //[label][IdentityInfo]
        private Dictionary<int, IdentityInfo> identities;

        /// <summary>
        /// Currently loaded identities.
        /// Key: Unique label
        /// Value: IdentityInfo object
        /// </summary>
        public Dictionary<int, IdentityInfo> Identities { get; }

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

        /// <summary>
        /// Initialize this library instance.
        /// </summary>
        /// <param name="classifierConfigFile">Path to the classifier configuration file.</param>
        /// <param name="recognizerSaveFile">Path to the recognizer's save file.</param>
        /// <param name="identitySaveFile">Path to the identity save file</param>
        public FaceRecLibrary(string classifierConfigFile = null, string recognizerSaveFile = null, string identitySaveFile = null, IMetadataHandler metadataHandler = null)
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
            {
                identities = new Dictionary<int, IdentityInfo>();
                List<IdentityInfo> newList = (List<IdentityInfo>)Util.LoadFromXml(typeof(List<IdentityInfo>), identitySaveFile);
                if(newList != null)
                    foreach (IdentityInfo id in newList)
                    {
                        if(id.Label != null)
                            identities.Add(id.Label.Value, id);
                    }
            }
            else
                identities = new Dictionary<int, IdentityInfo>();
            if (metadataHandler != null)
                this.metadataHandler = metadataHandler;
            else
                this.metadataHandler = new XMPMetadataHandler();
        }

        /// <summary>
        /// Run automated face detection followed by face recognition.
        /// </summary>
        /// <param name="image">Image to process</param>
        public void DetectAndRecognize(ImageInfo image)
        {
            faceDetector.DetectFaces(image);
            try {
                if(faceRecognizer.Match(image) > 0)
                {
                    foreach(Detection d in image.Detections)
                    {
                        if (d.Identity?.Label != null)
                            d.Identity = identities[d.Identity.Label.Value];
                    }
                }
                    

            }
            catch (Exception) { }
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

        /// <summary>
        /// Adds manual detections to an image. 
        /// These detections are assumed to have been input manually by a user.
        /// </summary>
        /// <param name="image">Image to which detections are to be added.</param>
        /// <param name="detections">Detections to add to the image.</param>
        public void AddManualDetections(ImageInfo image, List<Detection> detections)
        {
            image.Detections.AddRange(detections);
            Util.MergeDuplicates(image.Detections);
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
            metadataHandler.Save(image, saveParams);
        }

        /// <summary>
        /// Load image metadata to the provided ImageInfo.
        /// </summary>
        /// <param name="image">Image of which to load metadata</param>
        /// <param name="loadParams">Parameters needed to load metadata (i.e: file path, connection string)</param>
        public void LoadMetadata(ImageInfo image, object loadParams)
        {
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
            return faceDetector.DetectFaces(image);
        }

        public void SaveIdentityInformation(string filePath)
        {
            Util.SaveToXml(identities.Values, filePath);
        }

        /// <summary>
        /// Run face recognizer on a single image.
        /// CPU-intensive, should be called from a worker thread.
        /// </summary>
        /// <param name="image">The ImageInfo to process</param>
        /// <returns>The number of recognitions</returns>
        public int RecognizeFaces(ImageInfo image)
        {
            int numRecognized = faceRecognizer.Match(image);
            if(numRecognized > 0)
            {
                foreach(Detection d in image.Detections)
                {
                    if (!identities.ContainsKey(d.Identity.Label ?? -1))
                        identities.Add(d.Identity.Label ?? -1, d.Identity);
                }
            }
            return numRecognized;
        }

        /// <summary>
        /// Gets all identities associated with a name.
        /// </summary>
        /// <param name="name">The name to search for</param>
        /// <returns>An array of IdentityInfo instances registered for the give name or an empty list if there aren't any.</returns>
        public IdentityInfo[] GetIdsFromName(string name)
        {
            List<IdentityInfo> ids = new List<IdentityInfo>();
            foreach(IdentityInfo id in identities.Values)
            {
                if (id.Name.Equals(name))
                {
                    ids.Add(id);
                }
            }
            return ids.ToArray();
        }

        /// <summary>
        /// Add detection information to an image or update and existing detection with new identity info.
        /// Also updates face recognizer with the new information.
        /// </summary>
        /// <param name="imageInfo">Image to which the detection is to be added.</param>
        /// <param name="detection">Detection to add.</param>
        public void AddOrUpdateDetection(ImageInfo imageInfo, Detection detection)
        {
            foreach(Detection d in imageInfo.Detections)
            {
                if (d.Area == detection.Area && detection.Identity != null)
                    d.Identity = detection.Identity;
            }
            if (detection.Identity.Label != null) {
                if (!identities.ContainsKey(detection.Identity.Label.Value))
                    identities.Add(detection.Identity.Label.Value, detection.Identity);
                else
                    identities[detection.Identity.Label.Value].AddDetection(detection);
                }
            else
            {
                //Generate label
                int newLabel = 0;
                foreach (IdentityInfo id in identities.Values)
                {
                    newLabel = Math.Max(id.Label ?? 0, newLabel);
                }
                detection.Identity.Label = newLabel;

                //Register Identity
                identities.Add(newLabel, detection.Identity);
            }
            //Update Recognizer if there are enough detections            
            if (identities[detection.Identity.Label.Value].Detections.Count > 6)
            {
                if (faceRecognizer.IsTrained)
                {
                    faceRecognizer.UpdateRecognizer(detection.Identity);
                }
                else
                {
                    if (identities.Count > 9 && identities.Values.Count((id) => id.Detections.Count > 6) > 9)
                    {
                        faceRecognizer.TrainRecognizer(identities.Values.Where((id) => id.Detections.Count > 6));
                    }
                }
            }

        }
    }
}
