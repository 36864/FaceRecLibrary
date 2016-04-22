using FaceRecLibrary;
using OpenCvSharp.CPlusPlus;
using OpenCvSharp;
using System.IO;

namespace FaceRecTest
{
    class TestDetection
    {
        const string CLASSIFIER_BASE_PATH = "Data/Classifier/";
        const string IMAGE_BASE_PATH = "Data/Image/";
        const string IMAGE_LIST = "detection_test_image_list.txt";
        private static string[] CLASSIFIERS =
            {
                CLASSIFIER_BASE_PATH + "haarcascade_frontalface_alt.xml",
                CLASSIFIER_BASE_PATH + "haarcascade_frontalface_alt_tree.xml",
                CLASSIFIER_BASE_PATH + "haarcascade_frontalface_alt2.xml",
                CLASSIFIER_BASE_PATH + "haarcascade_frontalface_default.xml",
                CLASSIFIER_BASE_PATH + "haarcascade_profileface.xml",
                CLASSIFIER_BASE_PATH + "lbpcascade_frontalface.xml",
                CLASSIFIER_BASE_PATH + "lbpcascade_profileface.xml",
            };
        


        public static void Main(string[] args)
        {
            string[] image_list = Util.read_list(IMAGE_BASE_PATH + IMAGE_LIST);
            foreach(string path in image_list) { 
                using (var img = Cv2.ImRead(IMAGE_BASE_PATH + path, LoadMode.GrayScale))
                {
                    Rect[][] results = FaceDetect.RunDetection(img, CLASSIFIERS);
                    for (int j = 0; j < results.Length; ++j)
                    {
                        Rect[] resultSet = results[j];
                        foreach (var rect in resultSet)
                        {
                            using (var img_result = Cv2.ImRead(IMAGE_BASE_PATH + path))
                            {
                                Cv.Rectangle(img_result.ToCvMat(), rect, CvColor.AntiqueWhite, 5);
                                string result_path = "Results/" + CLASSIFIERS[j].Substring(CLASSIFIER_BASE_PATH.Length).Replace(".xml", "/");
                                Directory.CreateDirectory(result_path + path.Substring(0, path.IndexOf('/')));
                                Cv.SaveImage(result_path + path, img_result.ToCvMat());
                            }
                        }
                    }                    
                }
            }
        }
    }
}

