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
        public static int Match(Mat to_check, FaceRecognizer reference, double confidence_threshold = 5000)
        {
            int predicted_label = -1;
            double confidence = 0.0;
            reference.Predict(to_check, out predicted_label, out confidence);
            if (confidence <= confidence_threshold)
                return predicted_label;
            else
                return -1;
        }

        public static FaceRecognizer[] TrainRecognizers(List<Mat> training_set, List<int> labels)
        {
            FaceRecognizer[] result = new FaceRecognizer[3];
            for(int i = 0; i < result.Length; ++i)
            {
                result[i] = TrainRecognizer((FaceRecType) i, training_set, labels);
            }
            return result;
        }


        /// <summary>
        /// Train a new FaceRecognizer using the specified training set.
        /// </summary>
        /// <param name="training_set">Training set to train the FaceRecognizer with. All images must have the same dimensions</param>
        /// <param name="labels">Labels for each image in the training set</param>
        /// <returns>A FaceRecognizer instance if training was successful</returns>        
        public static FaceRecognizer TrainRecognizer(FaceRecType type, List<Mat> training_set, List<int> labels)
        {
            //Create new FaceRecognizer instance to train. Currently this is set to an EigenFace recognizer, pending ability to let the user choose
            FaceRecognizer new_recognizer;
            switch (type)
            {
                case FaceRecType.Eigen: new_recognizer = FaceRecognizer.CreateEigenFaceRecognizer(); break;
                case FaceRecType.Fisher: new_recognizer = FaceRecognizer.CreateFisherFaceRecognizer(); break;
                case FaceRecType.LBPH: new_recognizer = FaceRecognizer.CreateLBPHFaceRecognizer(); break;
                default: throw new InvalidOperationException("Invalid FaceRecType parameter");
            }
            //Remove one image from the training set to verify it works after training
            Mat test_sample = training_set[training_set.Count - 1];
            int test_label = labels[labels.Count - 1];
            training_set.RemoveAt(training_set.Count - 1);
            labels.RemoveAt(labels.Count - 1);

            //Train the recognizer
            new_recognizer.Train(training_set, labels);

            //Test it with one of the training images. 
            //If the recognizer fails to recognize a training image, more training images might be needed or the training
            //image may have been incorrect
            if (test_label != new_recognizer.Predict(test_sample))
                throw new System.Exception("FaceRecognizer training failed - test sample misclassified. Add more images to training set and try again.");

            return new_recognizer;
        }

        public static FaceRecognizer UpdateRecognizer(FaceRecognizer to_update, List<Mat> new_data_set, List<int> labels)
        {
            throw new NotImplementedException();
            //This may be useful in the future.
            //to_update.Update((IEnumerable<Mat>)new_data_set, (IEnumerable<int>)labels);
            //    return to_update;
        }

    }
}
