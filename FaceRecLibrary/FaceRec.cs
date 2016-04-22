using OpenCvSharp;
using OpenCvSharp.CPlusPlus;
using System.Collections;
using System.Collections.Generic;
using System;

namespace FaceRecLibrary
{
    public class FaceRec
    {
        /// <summary>
        /// Check if the specified image is recognizeable by the specified FaceRecognizer
        /// </summary>
        /// <param name="to_check">Image to be checked</param>
        /// <param name="reference">Previously trained FaceRecognizer</param>
        /// <param name="confidence_threshold">Maximum distance (confidence) for which to accept a match</param>
        /// <returns>The label that the FaceRecognizer predicts for the specified image. -1 means there was no match or the match was made with a confidence value exceeding the confidence threshold</returns>
        public static int Match(Mat to_check, FaceRecognizer reference, double confidence_threshold = 8000)
        {
            int predicted_label = -1;
            double confidence = 0.0;
            reference.Predict(to_check, out predicted_label, out confidence);
            if (confidence <= confidence_threshold)
                return predicted_label;
            else
                return -1;
        }

        /// <summary>
        /// Train a new FaceRecognizer using the specified training set.
        /// </summary>
        /// <param name="training_set">Training set to train the FaceRecognizer with. All images must have the same dimensions</param>
        /// <param name="labels">Labels for each image in the training set</param>
        /// <returns>A FaceRecognizer instance if training was successful</returns>        
        public static FaceRecognizer TrainRecognizer( List<Mat> training_set, List<int> labels)
        {
            //Create new FaceRecognizer instance to train. Currently this is set to an EigenFace recognizer, pending ability to let the user choose
            FaceRecognizer new_recognizer = FaceRecognizer.CreateEigenFaceRecognizer();

            //Remove one image from the training set to verify it works after training
            Mat test_sample = training_set[training_set.Count - 1];
            int test_label = labels[labels.Count - 1];
            training_set.RemoveAt(training_set.Count - 1);
            labels.RemoveAt(labels.Count - 1);

            //Train the recognizer
            new_recognizer.Train( training_set, labels);

            //Test it with one of the training images. 
            //If the recognizer fails to recognize a training image, more training images might be needed or the training
            //image may have been incorrect
            if (test_label != new_recognizer.Predict(test_sample))
                throw new System.Exception("EigenFace training failed - test sample misclassified. Add more images to training set and try again.");

            return new_recognizer;
        }

        public static FaceRecognizer UpdateRecognizer(FaceRecognizer to_update, ArrayList new_data_set, ArrayList labels)
        {
            throw new NotImplementedException();
            //This may be useful in the future.
            //to_update.Update((IEnumerable<Mat>)new_data_set, (IEnumerable<int>)labels);
        //    return to_update;
        }
        
    }
}
