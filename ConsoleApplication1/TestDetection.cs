using FaceRecLibrary;
using OpenCvSharp.CPlusPlus;
using OpenCvSharp;
using System.IO;
using System;

namespace FaceRecTest
{
    class TestDetection
    {
        const string CLASSIFIER_BASE_PATH = "../../Data/Classifier/";
        const string IMAGE_BASE_PATH = "../../Data/Image/";
        const string RESULT_BASE_PATH = "../../Results/";
        const string IMAGE_LIST = "detection_test_image_list.txt";
        const string CLASSIFIER_LIST = "classifier_list.txt";
        const double DEFAULT_SCALE = 1.08;
        const int DEFAULT_MIN_NEIGHBORS = 4;

        private static Size MAX_IMG_SIZE = new Size(200, 200);
        private static string[] CLASSIFIERS;
        private static string[] RESULT_PATHS;
        private static double[] SCALES;
        private static int[] MIN_NEIGHBORS;

        public static void Main(string[] args)
        {
            //Read classifier list
            CLASSIFIERS = Util.read_list(CLASSIFIER_BASE_PATH + CLASSIFIER_LIST);            
            RESULT_PATHS = new string[CLASSIFIERS.Length];
            SCALES = new double[CLASSIFIERS.Length];
            MIN_NEIGHBORS = new int[CLASSIFIERS.Length];
            //Build classifier result paths
            for (int i = 0; i < CLASSIFIERS.Length; i++)
            {
                try {
                    string scale = CLASSIFIERS[i].Substring(CLASSIFIERS[i].IndexOf(' '), CLASSIFIERS[i].LastIndexOf(' ') - CLASSIFIERS[i].IndexOf(' '));
                    SCALES[i] = double.Parse(scale);
                    MIN_NEIGHBORS[i] = int.Parse(CLASSIFIERS[i].Substring(CLASSIFIERS[i].LastIndexOf(' ')));
                    CLASSIFIERS[i] = CLASSIFIER_BASE_PATH + CLASSIFIERS[i].Substring(0, CLASSIFIERS[i].IndexOf(' '));
                    RESULT_PATHS[i] = RESULT_BASE_PATH + CLASSIFIERS[i].Substring(CLASSIFIER_BASE_PATH.Length).Replace(".xml", "/");
                }
                catch(Exception)
                {
                    SCALES[i] = DEFAULT_SCALE;
                    MIN_NEIGHBORS[i] = DEFAULT_MIN_NEIGHBORS;
                    CLASSIFIERS[i] = CLASSIFIER_BASE_PATH + CLASSIFIERS[i];
                    RESULT_PATHS[i] = RESULT_BASE_PATH + CLASSIFIERS[i].Substring(CLASSIFIER_BASE_PATH.Length).Replace(".xml", "/");
                }
            }

            //Read test image list
            string[] image_list = Util.read_list(IMAGE_BASE_PATH + IMAGE_LIST);
            Mat img;
            Rect[][] results;
            int img_scale;
            foreach (string path in image_list)
            {

                //Read image from file
                img = Cv2.ImRead(IMAGE_BASE_PATH + path, LoadMode.GrayScale);

                //resize image
                img_scale = 1;
                while (img.Rows > MAX_IMG_SIZE.Height && img.Cols > MAX_IMG_SIZE.Width)
                {
                    img_scale *= 2;
                    img = img.Resize(Size.Zero, 0.5f, 0.5f);
                }

                //Run facial detection
                results = FaceDetect.RunDetection(img, CLASSIFIERS, SCALES, MIN_NEIGHBORS);

                //Save results
                for (int j = 0; j < results.Length; ++j)
                {
                    Rect[] resultSet = results[j];
                    for (int i = 0; i < resultSet.Length; ++i)
                    {
                        Rect rect = resultSet[i];
                        rect.X *= img_scale;
                        rect.Y *= img_scale;
                        rect.Width *= img_scale;
                        rect.Height *= img_scale;
                        using (var img_result = Cv2.ImRead(IMAGE_BASE_PATH + path).SubMat(rect))
                        {
                            //Create directory if it doesn't exist yet
                            Directory.CreateDirectory(RESULT_PATHS[j] + path.Substring(0, path.LastIndexOf('/')));

                            //Save cropped image result
                            Cv.SaveImage(RESULT_PATHS[j] + path.Replace(path.Substring(path.LastIndexOf('.')), i + path.Substring(path.LastIndexOf('.'))), img_result.ToCvMat());
                        }
                    }
                }
                img.Release();
                results = null;
            }
        }
    }
}


