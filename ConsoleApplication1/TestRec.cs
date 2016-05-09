using System.Collections.Generic;
using FaceRecLibrary;
using OpenCvSharp;
using OpenCvSharp.CPlusPlus;
using System;

namespace FaceRecTest
{
    class TestRec
    {
        private static string image_base_path = "../../Data/Image/";
        private static string training_set_file_list = "recognition_training_set.txt";
        private static string test_image_list = "recognition_test_images.txt";




        public static void Main(string[] args)
        {
            //Read image path list
            string[] files = Util.read_list(image_base_path + training_set_file_list);
            
            //Load training set images
            List<Mat> training_set = new List<Mat>();
            List<int> labels = new List<int>();
            for (int i = 0; i < files.Length; i++)
            {
                string path = files[i].Substring(0, files[i].IndexOf(' '));
                int label = int.Parse(files[i].Substring(files[i].IndexOf(' ')));
                training_set.Add(Cv2.ImRead(image_base_path + path, LoadMode.GrayScale));
                labels.Add(label);
            }

            //Train recognizers
            FaceRecognizer[] frecs = FaceRec.TrainRecognizers(training_set, labels);



            //Test recognizers
            string[] test_images = Util.read_list(image_base_path + test_image_list);
            foreach (var frec in frecs)
            {
                foreach(var param in frec.GetParams())
                {

                    Console.WriteLine(param);
                }
                for (int i = 0; i < test_images.Length; ++i)
                {
                    int expected = int.Parse(test_images[i].Substring(test_images[i].IndexOf(' ')));
                    string path = test_images[i].Substring(0, test_images[i].IndexOf(' '));

                    Console.WriteLine("Expected " + expected
                        + ". Got " + FaceRec.Match(Cv2.ImRead(image_base_path + path, LoadMode.GrayScale), frec, 5000) +
                        " using " + frec.Info.Name);
                }

            }
            


            Console.ReadLine();
        }
    }
}
