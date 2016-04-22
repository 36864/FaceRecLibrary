using System.Collections.Generic;
using FaceRecLibrary;
using OpenCvSharp;
using OpenCvSharp.CPlusPlus;
using System;

namespace FaceRecTest
{
    class TestRec
    {
        private static string image_base_path = "Data/Image/CroppedYale/";
        private static string file_list = "yaleB1/yaleB1_P00.info";
        private static string test_image = "yaleB1/yaleB01_P00A-130E+20.pgm";

      

        public static void Main(string[] args)
        {
            //Read image path list
            string[] files = Util.read_list(image_base_path + file_list);

            //Load training set images
            List<Mat> training_set = new List<Mat>();
            List<int> labels = new List<int>();
            for (int i = 0; i < files.Length; i++)
            {
                training_set.Add(Cv2.ImRead(image_base_path + "yaleB1/" + files[i], LoadMode.GrayScale));
                labels.Add(1);
            }

            //Train recognizer
            FaceRecognizer frec = FaceRec.TrainRecognizer(training_set, labels);

            //Test recognizer
            Console.WriteLine("Expected 1. Got " + FaceRec.Match(Cv2.ImRead(image_base_path + test_image, LoadMode.GrayScale).EqualizeHist(), frec, 5000) + ".");            
        }
    }
}
