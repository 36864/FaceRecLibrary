using OpenCvSharp.CPlusPlus;

namespace FaceRecLibrary
{
    public class FaceDetect
    {
        const double DEFAULT_SCALE = 1.08;
        const int DEFAULT_MIN_NEIGHBORS = 4;

        /// <summary>
        /// Runs facial detection on the specified image using all the specified classifiers.
        /// Classifiers are loaded from their file paths.
        /// </summary>
        /// <param name="img"></param>
        /// <param name="classifier_paths"></param>
        /// <param name="scale_factor"></param>
        /// <param name="min_neighbors"></param>
        /// <returns>Detected face positions as rectangles, by classifier (return_value[0][1] is the second face detected by the first classifier)</returns>
        public static Rect[][] RunDetection(Mat img, string[] classifier_paths, double[] scale_factor = null, int[] min_neighbors = null)
        {
            Rect[][] detections = new Rect[classifier_paths.Length][];
            for (int i = 0; i < classifier_paths.Length; ++i)
            {
                //Load classifier from classifier file (.xml)
                using (CascadeClassifier classifier = new CascadeClassifier(classifier_paths[i]))
                {
                    //Run classifier
                    if (scale_factor == null && min_neighbors == null)
                        detections[i] = RunDetection(img, classifier);
                    else if (min_neighbors == null)
                        detections[i] = RunDetection(img, classifier, scale_factor[i]);
                    else if (scale_factor == null)
                        detections[i] = RunDetection(img, classifier, DEFAULT_SCALE, min_neighbors[i]);
                    else
                        detections[i] = RunDetection(img, classifier, scale_factor[i], min_neighbors[i]);
                }
            }
            return detections;
        }

        /// <summary>
        /// Runs facial detection on the specified image using the specified classifier.        
        /// </summary>
        /// <param name="img"></param>
        /// <param name="classifier_paths"></param>
        /// <param name="scale_factor"></param>
        /// <param name="min_neighbors"></param>
        /// <returns>Detected face positions as rectangles, by classifier (return_value[0][1] is the second face detected by the first classifier)</returns>
        public static Rect[] RunDetection(Mat img, CascadeClassifier classifier, double scale_factor = DEFAULT_SCALE, int min_neighbors = DEFAULT_MIN_NEIGHBORS)
        {
            return classifier.DetectMultiScale(img, scale_factor, min_neighbors);
        }

        public static Rect[][] RunDetection(Mat img, ClassifierInfo[] classifiers)
        {
            Rect[][] detections = new Rect[classifiers.Length][];
            for (int i = 0; i < classifiers.Length; ++i)
            {
                //Load classifier from classifier file (.xml)
                using (CascadeClassifier classifier = new CascadeClassifier(classifiers[i].FullName))
                {
                    //Run classifier
                    detections[i] = RunDetection(img, classifier);
                }
            }
            return detections;
        }

        public static Rect[] RunDetection(Mat img, ClassifierInfo classifier)
        {
            if (classifier.Scale < 1)
                classifier.Scale = DEFAULT_SCALE;
            if (classifier.MinNeighbors < 1)
                classifier.MinNeighbors = DEFAULT_MIN_NEIGHBORS;
            using (CascadeClassifier loadedClassifier = new CascadeClassifier(classifier.Path))
            {
                return loadedClassifier.DetectMultiScale(img, classifier.Scale, classifier.MinNeighbors);
            }
        }
    }
}
