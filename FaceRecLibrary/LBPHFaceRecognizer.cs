using OpenCvSharp.CPlusPlus;
using System.Collections.Generic;
using FaceRecLibrary.Utilities;

namespace FaceRecLibrary
{
    public class LBPHFaceRecognizer
    {

        private FaceRecognizer recognizer;

        public bool IsTrained { get; set; }

        public LBPHFaceRecognizer()
        {
            recognizer = FaceRecognizer.CreateLBPHFaceRecognizer();
            IsTrained = false;
        }

        public LBPHFaceRecognizer(string filename)
        {
            recognizer = FaceRecognizer.CreateLBPHFaceRecognizer();
            recognizer.Load(filename);
            IsTrained = false;
        }

        public void Load(string filename)
        {
            recognizer.Load(filename);
        }

        public void Save(string filename)
        {
            recognizer.Save(filename);
        }

        /// <summary>
        /// Adds identity information (if available) to detections in the given ImageInfo.
        /// Should be called after detection info has been obtained by calling FaceDetect.RunDetection().
        /// </summary>
        /// <param name="img">The ImageInfo for which to acquire identity information.</param>
        public void Match(ImageInfo img)
        {
            if (FaceRec.Match(this.recognizer, img))
            {
                List<ImageInfo> l = new List<ImageInfo>();
                l.Add(img);
                UpdateRecognizer(l);
            }
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
            foreach (ImageInfo image in images) {
                UpdateRecognizer(image);
            }            
        }

        public void UpdateRecognizer(ImageInfo image)
        {
            List<Mat> faces = new List<Mat>();
            List<int> tags = new List<int>();
            foreach (Detection detection in image.DetectionInfo.Detections)
            {
                if (detection.Identity?.Label != null)
                    using (Mat mat = new Mat(new Mat(image.Path), Util.CvtRectangletoRect(detection.Area)))
                    {
                        faces.Add(mat);
                        tags.Add(detection.Identity.Label);
                    }
            }
            recognizer.Update(faces, tags);
        }
    }
}
