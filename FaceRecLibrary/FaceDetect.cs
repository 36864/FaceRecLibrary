using OpenCvSharp;
using OpenCvSharp.CPlusPlus;

namespace FaceRecLibrary
{
    public class FaceDetect
    {
        public static Rect[][] RunDetection(string image_filepath, LoadMode mode, string[] classifier_paths)
        {
            using (var img = new Mat(image_filepath, mode))
                return RunDetection(img, classifier_paths);
        }

        public static Rect[][] RunDetection(Mat img, string[] classifier_paths, double scale_factor = 1.1, int min_neighbors = 10)
        {
            Rect[][] detections = new Rect[classifier_paths.Length][];
            for(int i = 0; i < classifier_paths.Length; ++i)
            {
                using (CascadeClassifier classifier = new CascadeClassifier(classifier_paths[i]))
                {
                    detections[i] = classifier.DetectMultiScale(img, scale_factor, min_neighbors);
                }
            }
            return detections;
        }
    }
}
