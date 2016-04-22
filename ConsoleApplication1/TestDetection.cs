using FaceRecLibrary;
using OpenCvSharp.CPlusPlus;
using OpenCvSharp;
using System.IO;

namespace FaceRecTest
{
    class TestDetection
    {
        const string CLASSIFIER_BASE_PATH = "../../Data/Classifier/";
        const string IMAGE_BASE_PATH = "../../Data/Image/";
        const string RESULT_BASE_PATH = "../../Results/";
        const string IMAGE_LIST = "detection_test_image_list.txt";
        const string CLASSIFIER_LIST = "classifier_list.txt";
        private static string[] CLASSIFIERS;
        private static string[] RESULT_PATHS;
        


        public static void Main(string[] args)
        {
            //Read classifier list
            CLASSIFIERS = Util.read_list(CLASSIFIER_BASE_PATH + CLASSIFIER_LIST);

            //Build classifier result paths
            for (int i = 0; i < CLASSIFIERS.Length; i++)
            {
                RESULT_PATHS[i] = RESULT_BASE_PATH + CLASSIFIERS[i].Substring(CLASSIFIER_BASE_PATH.Length).Replace(".xml", "/");
            }

            //Read test image list
            string[] image_list = Util.read_list(IMAGE_BASE_PATH + IMAGE_LIST);

            foreach (string path in image_list) {
                //Read image from file
                using (var img = Cv2.ImRead(IMAGE_BASE_PATH + path, LoadMode.GrayScale))
                {
                    //Run facial detection
                    Rect[][] results = FaceDetect.RunDetection(img, CLASSIFIERS);

                    //Save results
                    for (int j = 0; j < results.Length; ++j)
                    {
                        Rect[] resultSet = results[j];
                        foreach (var rect in resultSet)
                        {
                            //Read image, crop to detected face
                            using (var img_result = Cv2.ImRead(IMAGE_BASE_PATH + path).SubMat(rect))
                            {
                                //Create directory if it doesn't exist yet
                                Directory.CreateDirectory(RESULT_PATHS[j] + path.Substring(0, path.IndexOf('/')));

                                //Save cropped image result
                                Cv.SaveImage(RESULT_PATHS[j] + path, img_result.ToCvMat());
                            }
                        }
                    }
                }

                }
            }
        }
    }
}

