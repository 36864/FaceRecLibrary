using OpenCvSharp;
using OpenCvSharp.CPlusPlus;
using System.Collections;
using System.Collections.Generic;
using System;

namespace FaceRecLibrary
{
    public class FaceRec
    {
        //public static Dictionary<FaceRecognizer, double> recsDist = new Dictionary<FaceRecognizer, double>();
        
        /// <summary>
        /// Check if the specified image is recognizeable by the specified FaceRecognizer
        /// </summary>
        /// <param name="to_check">Image to be checked</param>
        /// <param name="reference">Previously trained FaceRecognizer</param>
        /// <param name="confidence_threshold">Maximum distance (confidence) for which to accept a match</param>
        /// <returns>The label that the FaceRecognizer predicts for the specified image. -1 means there was no match or the match was made with a confidence value exceeding the confidence threshold</returns>
        public static int Match(Mat to_check, FaceRecognizer reference/*, double confidence_threshold = 5000*/)
        {
            //int predicted_label = 0;
            //double confidence = 0.0;

            return reference.Predict(to_check/*, out predicted_label, out confidence*/);

            //Tests if the new confidence is bigger than previous added, if not exists adds a new element to the dictionary
     /*       if (recsDist.ContainsKey(reference))
                recsDist[reference] = (recsDist[reference] < confidence) ? confidence : recsDist[reference];
            else
                recsDist.Add(reference, confidence);*/
                        
           //return predicted_label = (confidence <= confidence_threshold) ? predicted_label : -1;
            /*
            if (confidence <= confidence_threshold)
                return predicted_label;
            else
                return -1;
            */
        }

        public static FaceRecognizer[] TrainRecognizers(List<Mat> training_set, List<int> labels)
        {
            FaceRecognizer[] result = new FaceRecognizer[3];
            for (int i = 0; i < result.Length; ++i)
            {
                result[i] = TrainRecognizer((FaceRecType)i, training_set, labels);
            }
            return result;
        }


        /// <summary>
        /// Train a new FaceRecognizer using the specified training set.
        /// Training set must be ordered by label
        /// </summary>
        /// <param name="training_set">Training set to train the FaceRecognizer with. All images must have the same dimensions</param>
        /// <param name="labels">Labels for each image in the training set</param>
        /// <returns>A FaceRecognizer instance if training was successful</returns>        
        public static FaceRecognizer TrainRecognizer(FaceRecType type, List<Mat> training_set, List<int> labels)
        {
            //Create new FaceRecognizer instance to train. Currently this is set to an EigenFace recognizer, pending ability to let the user choose
            FaceRecognizer new_recognizer;
            List<int> local_labels = new List<int>(labels);
            List<Mat> local_training_set = new List<Mat>(training_set);
            

            switch (type)
            {
                case FaceRecType.Eigen: new_recognizer = FaceRecognizer.CreateEigenFaceRecognizer(); break;
                case FaceRecType.Fisher: new_recognizer = FaceRecognizer.CreateFisherFaceRecognizer(); break;
                case FaceRecType.LBPH: new_recognizer = FaceRecognizer.CreateLBPHFaceRecognizer(); break;
                default: throw new InvalidOperationException("Invalid FaceRecType parameter");
            }
            //Remove one image per label from the training set for testing and thresholding
            int i = 0;
            List<int> test_indexes = new List<int>();
            while(i < local_labels.Count)
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
            new_recognizer.Train(local_training_set, local_labels);

            //Find max distance within training set
            double max_distance = 0.0, distance = 0.0;
            int label = -1;
            for (i = 0; i < test_indexes.Count; i++)
            {
                new_recognizer.Predict(test_images[i], out label, out distance);
                if (label != test_labels[i])
                    throw new Exception("Training failed. Error in training set.");
                if (distance > max_distance)
                    max_distance = distance;
            }
            
            //Set confidencet threshold based on max distance within training set
            new_recognizer.SetDouble("threshold", max_distance*1.51);
            Console.WriteLine(new_recognizer.Name + " trained. Threshold set to " + max_distance*1.51);

            return new_recognizer;
        }

        public static FaceRecognizer UpdateRecognizer(FaceRecognizer to_update, List<Mat> new_data_set, List<int> labels)
        {
            //This may be useful in the future.
            //            if(to_update.Name.Substring(to_update.Name.IndexOf(".")).Equals("LBPH")) nao sei se preferes uma verificacao mais segura tipo esta

            if (to_update.Name.Contains("LBPH"))
                to_update.Update(new_data_set, labels);
            return to_update;
        }

    }
}
