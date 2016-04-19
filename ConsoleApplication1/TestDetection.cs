using FaceRecLibrary;
using OpenCvSharp.CPlusPlus;
using OpenCvSharp;

namespace FaceRecTest
{
    class TestDetection
    {
        private static string image_base_path = "Data/Image/";
        private static string Classifier_Path_Haar = "Data/Classifier/haarcascade_frontalface_default.xml";
        private static string Classifier_Path_Haar_alt = "Data/Classifier/haarcascade_frontalface_alt.xml";
        private static string Classifier_Path_LBP = "Data/Classifier/lbpcascade_frontalface.xml";        


        public static void Main(string[] args)
        {
            string[] classifiers = { Classifier_Path_Haar, Classifier_Path_Haar_alt, Classifier_Path_LBP};

            for(int i = 1; i <= 13; ++i) {
                string image_Path = image_base_path + i + ".jpg";
                using (var img = new Mat(image_Path, LoadMode.GrayScale))
                {
                    Rect[][] results = FaceDetect.RunDetection(img, classifiers);
                    for (int j = 0; j < results.Length; ++j)
                    {
                        Rect[] resultSet = results[j];
                        foreach (var rect in resultSet)
                        {
                            Cv.Rectangle(img.ToCvMat(), rect, CvColor.AntiqueWhite, j * 5 + 1);
                        }
                    }
                    Cv.SaveImage("Result" + i + ".jpg", img.ToCvMat());
                    Cv.WaitKey(0);
                }
            }
        }
    }
}

