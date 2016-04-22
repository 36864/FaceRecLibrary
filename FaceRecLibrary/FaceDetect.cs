using OpenCvSharp;
using OpenCvSharp.CPlusPlus;

namespace FaceRecLibrary
{
    public class FaceDetect
    {
        public static Rect[][] RunDetection(string image_filepath, LoadMode mode, string[] classifier_paths, double scale_factor = 1.1, int min_neighbors = 10)
        {
            using (var img = new Mat(image_filepath, mode))
                return RunDetection(img, classifier_paths);
        }


        /// <summary>
        /// Runs facial detection on the specified image using all the specified classifiers.
        /// Classifiers are loaded from their file paths.
        /// Scale factor and minimum neighbor values are optional and used for every classifier (for now?)
        /// </summary>
        /// <param name="img"></param>
        /// <param name="classifier_paths"></param>
        /// <param name="scale_factor"></param>
        /// <param name="min_neighbors"></param>
        /// <returns>Detected face positions as rectangles, by classifier (return_value[0][1] is the second face detected by the first classifier)</returns>
        public static Rect[][] RunDetection(Mat img, string[] classifier_paths, double scale_factor = 1.1, int min_neighbors = 10)
        {
            Rect[][] detections = new Rect[classifier_paths.Length][];
            for(int i = 0; i < classifier_paths.Length; ++i)
            {
                //Load classifier from classifier file (.xml)
                using (CascadeClassifier classifier = new CascadeClassifier(classifier_paths[i]))
                {
                    //Run classifier
                    detections[i] = classifier.DetectMultiScale(img, scale_factor, min_neighbors);
                }
            }
            return detections;
        }
    }
}
