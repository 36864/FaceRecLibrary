using OpenCvSharp.CPlusPlus;
using System.Collections.Generic;
using System;
using FaceRecLibrary.Utilities;

namespace FaceRecLibrary
{
    public class FaceRec
    {

        //Dimensions used for face recognition - all face images will be resized to this size before being matched
        public static Size FaceDimensions = new Size(32, 32);
          
        public static void Load(FaceRecognizer recognizer, string filename)
        {
            recognizer.Load(filename);
        }

        public static void Save(FaceRecognizer recognizer, string filename)
        {
            recognizer.Save(filename);
        }

        /// <summary>
        /// Adds identity information (if available) to detections in the given ImageInfo.
        /// Should be called after detection info has been obtained by calling FaceDetect.RunDetection().
        /// </summary>
        /// <param name="img">The ImageInfo for which to acquire identity information.</param>
        public static int Match(FaceRecognizer recognizer, ImageInfo img)
        {
            int predicted_label = -1;
            double confidence = 0.0;
            int identified = 0;
            using (Mat original = new Mat(img.OriginalPath))
            {
                foreach (var detection in img.DetectionInfo.Detections)
                {
                    if (detection.Identity?.Label == null)
                        using (Mat to_check = new Mat(original, Util.CvtRectangletoRect(detection.Area)).Resize(FaceDimensions))
                            try {
                                recognizer.Predict(to_check, out predicted_label, out confidence);
                            }
                            catch (Exception)
                            {
                                predicted_label = -1;
                            }
                    if (predicted_label != -1)
                    {
                        ++identified;
                        if (detection.Identity == null)
                            detection.Identity = new IdentityInfo();
                        detection.Identity.Label = predicted_label;
                    }
                }
            }
            return identified;
        }

        /// <summary>
        /// Train a new FaceRecognizer using the specified training set.
        /// Training set must be ordered by label
        /// </summary>
        /// <param name="training_set">Training set to train the FaceRecognizer with. All images must have the same dimensions</param>
        /// <param name="labels">Labels for each image in the training set</param>
        /// <returns>A FaceRecognizer instance if training was successful</returns>        
        public static void TrainRecognizer(FaceRecognizer recognizer, List<Mat> training_set, List<int> labels)
        {
            List<int> local_labels = new List<int>(labels);
            List<Mat> local_training_set = new List<Mat>(training_set);
            foreach (Mat m in training_set) m.Resize(FaceDimensions);

            //Remove one image per label from the training set for testing and thresholding
            int i = 0;
            List<int> test_indexes = new List<int>();
            while (i < local_labels.Count)
            {
                i = local_labels.LastIndexOf(local_labels[i]);
                test_indexes.Add(i);
                ++i;
            }
            Mat[] test_images = new Mat[test_indexes.Count];
            int[] test_labels = new int[test_indexes.Count];
            for (i = test_indexes.Count - 1; i >= 0; --i)
            {
                test_images[i] = training_set[test_indexes[i]];
                test_labels[i] = local_labels[test_indexes[i]];
                local_training_set.RemoveAt(test_indexes[i]);
                local_labels.RemoveAt(test_indexes[i]);
            }

            //Train the recognizer
            recognizer.Train(local_training_set, local_labels);

            //Find max distance within training set
            double max_distance = 0.0, distance = 0.0;
            int label = -1;
            for (i = 0; i < test_indexes.Count; i++)
            {
                recognizer.Predict(test_images[i], out label, out distance);
                if (label != test_labels[i])
                    throw new Exception("Training failed. Error in training set.");
                if (distance > max_distance)
                    max_distance = distance;
            }

            //Set confidencet threshold based on max distance within training set
            recognizer.SetDouble("threshold", max_distance * 1.51);
            //Console.WriteLine(new_recognizer.Name + " trained. Threshold set to " + max_distance * 1.51;
        }
    }
}
