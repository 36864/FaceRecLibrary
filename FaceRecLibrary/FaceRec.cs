using OpenCvSharp;
using OpenCvSharp.CPlusPlus;
using System.Collections;
using System.Collections.Generic;

namespace FaceRecLibrary
{
    public class FaceRec
    {

        public static int Match(Mat to_check, FaceRecognizer reference, double confidence_threshold)
        {
            int predicted_label = -1;
            double confidence = 0.0;
            reference.Predict(to_check, out predicted_label, out confidence);
            if (confidence <= confidence_threshold)
                return predicted_label;
            else
                return -1;
        }

        public static FaceRecognizer TrainRecognizer( List<Mat> training_set, List<int> labels)
        {
            
            FaceRecognizer new_recognizer = FaceRecognizer.CreateEigenFaceRecognizer();
            Mat test_sample = training_set[training_set.Count - 1];
            int test_label = labels[labels.Count - 1];
            training_set.RemoveAt(training_set.Count - 1);
            labels.RemoveAt(labels.Count - 1);
            new_recognizer.Train( training_set, labels);

            if (test_label != new_recognizer.Predict(test_sample))
                throw new System.Exception("EigenFace training failed - test sample misclassified. Add more images to training set and try again.");
            return new_recognizer;
        }

        public static FaceRecognizer UpdateRecognizer(FaceRecognizer to_update, ArrayList new_data_set, ArrayList labels)
        {
            to_update.Update((IEnumerable<Mat>)new_data_set, (IEnumerable<int>)labels);
            return to_update;
        }
    }
}
