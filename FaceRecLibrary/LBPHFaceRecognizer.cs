using FaceRecLibrary.Types;
using FaceRecLibrary.Utilities;
using OpenCvSharp.CPlusPlus;
using System;
using System.Collections.Generic;
using System.IO;

namespace FaceRecLibrary
{
    public class LBPHFaceRecognizer
    {

        private FaceRecognizer recognizer;

        public bool IsTrained { get; set; }

        
        public LBPHFaceRecognizer()
        {
            recognizer = FaceRecognizer.CreateLBPHFaceRecognizer(1, 8, 8, 8, 500.0);
            IsTrained = false;
        }

        public LBPHFaceRecognizer(string filename)
        {
            recognizer = FaceRecognizer.CreateLBPHFaceRecognizer();
            try {
                recognizer.Load(filename);
                IsTrained = true;
            }
            catch (Exception)
            {
                IsTrained = false;
            }
        }

        public void Load(string filename)
        {
            recognizer.Load(filename);
        }

        public void Save(string filename)
        {
            if (!Directory.Exists(Path.GetDirectoryName(filename)))
                Directory.CreateDirectory(Path.GetDirectoryName(filename));
            recognizer.Save(filename);
        }

        /// <summary>
        /// Adds identity information (if available) to detections in the given ImageInfo.
        /// Should be called after detection info has been obtained by calling FaceDetect.RunDetection().
        /// </summary>
        /// <param name="img">The ImageInfo for which to acquire identity information.</param>
        public int Match(ImageInfo img)
        {
            if (!IsTrained)
                throw new System.Exception("Attempt to use untrained face recognizer");
            int matches = FaceRec.Match(recognizer, img);
            if (matches > 0)
            {
                List<ImageInfo> l = new List<ImageInfo>();
                l.Add(img);
                UpdateRecognizer(l);
            }
            return matches;
        }

        /// <summary>
        /// Train a new FaceRecognizer using the specified training set.
        /// Training set must be ordered by label
        /// </summary>
        /// <param name="training_set">Training set to train the FaceRecognizer with. All images must have the same dimensions</param>
        /// <param name="labels">Labels for each image in the training set</param>
        /// <returns>A FaceRecognizer instance if training was successful</returns>        
        public void TrainRecognizer(List<Mat> training_set, List<int> labels)
        {
            FaceRec.TrainRecognizer(recognizer, training_set, labels);
            IsTrained = true;
        }

        /// <summary>
        /// Update currently loaded recognizer with the detection info present in the given image list.
        /// Detections with unkown identities will be ignored.
        /// </summary>
        /// <param name="images">List of images with which to update the recognizer.</param>
        /// <remarks>Does not save the recognizer to file. Calling recognizer.Save(filename) after updating is recommended.</remarks>
        public void UpdateRecognizer(List<ImageInfo> images)
        {
            if (!IsTrained)
            {
                throw new Exception("Attempt to update an untrained recognizer");
            }
            foreach (ImageInfo image in images) {
                UpdateRecognizer(image);
            }            
        }

        public void UpdateRecognizer(ImageInfo image)
        {
            if (!IsTrained)
            {
                throw new Exception("Attempt to update an untrained recognizer");
            }
            List<Mat> faces = new List<Mat>();
            List<int> labels = new List<int>();
            using (Mat imageMat = new Mat(image.Path, OpenCvSharp.LoadMode.GrayScale)) {
                foreach (Detection detection in image.Detections)
                {
                    if (detection.Identity?.Label != null)
                    {
                        Mat mat = new Mat(imageMat, Util.CvtRectangletoRect(detection.Area));
                        mat.Resize(FaceRec.FaceDimensions);
                        faces.Add(mat);
                        labels.Add(detection.Identity.Label.Value);
                    }

                }
                if (!IsTrained)
                {
                    FaceRec.TrainRecognizer(recognizer, faces, labels);
                    IsTrained = true;
                }
                else
                    recognizer.Update(faces, labels);
            }
            foreach(Mat m in faces)
            {
                m.Dispose();
            }
        }

        internal void UpdateRecognizer(IdentityInfo identity)
        {
            if (!IsTrained)
            {
                throw new Exception("Attempt to update an untrained recognizer");
            }
            if (identity.Label == null) return;
            List<Mat> faces = new List<Mat>();
            List<int> labels = new List<int>();
            faces.AddRange(identity.GetFaceImages(FaceRec.FaceDimensions));
            foreach (var v in faces)
                labels.Add(identity.Label.Value);
            if (!IsTrained)
            {
                FaceRec.TrainRecognizer(recognizer, faces, labels);
                IsTrained = true;
            }
            else
                recognizer.Update(faces, labels);

            foreach (Mat m in faces)
            {
                m.Dispose();
            }
        }

        internal void TrainRecognizer(IEnumerable<IdentityInfo> identities)
        {
            List<Mat> faces = new List<Mat>();
            List<int> labels = new List<int>();
            foreach (IdentityInfo identity in identities)
            {
                Mat[] idFaces = identity.GetFaceImages(FaceRec.FaceDimensions);
                faces.AddRange(idFaces);
                foreach (var x in idFaces)
                    labels.Add(identity.Label.Value);
            }
            if (!IsTrained)
            {
                FaceRec.TrainRecognizer(recognizer, faces, labels);
                IsTrained = true;
            }
            else
                recognizer.Update(faces, labels);

            foreach (Mat m in faces)
            {
                m.Dispose();
            }
        }
    }
}
