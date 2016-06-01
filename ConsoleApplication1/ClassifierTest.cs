using System.IO;
using FaceRecLibrary;
using OpenCvSharp.CPlusPlus;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FaceRecTest
{
    class ClassifierTest
    {
        const string CLASSIFIER_BASE_PATH = "../../Data/Classifier/";
        const string IMAGE_BASE_PATH = "../../Data/Image/";
        const string RESULT_BASE_PATH = "../../Results/";

        public static void Main(string[] args)
        {
            string[] classifiers = Directory.GetFiles(CLASSIFIER_BASE_PATH, "*.xml");
            string[] images = Directory.GetFiles(IMAGE_BASE_PATH, "*", SearchOption.AllDirectories).Where(
                p => p.EndsWith(".jpg") || p.EndsWith(".png") || p.EndsWith(".jpeg")).ToArray();
            int[][] classifierDetections = new int[5][];
            ClassifierInfo[] cInfo = new ClassifierInfo[classifiers.Length];
            for (int i = 0; i < classifiers.Length; i++)
            {
                cInfo[i] = new ClassifierInfo(Path.GetFileName(classifiers[i]));
            }
            for(int i = 0; i < 5; ++i)
            {
                classifierDetections[i] = new int[classifiers.Length];
            }


            foreach (var imagePath in images)
            {           
                Mat image = new Mat(imagePath);
                
                for (int factor = 1; factor < 5; ++factor)
                {
                
                    for (double scale = 1.08; scale < 1.09; scale += 0.03)
                    {
                        for(int neighbors = 3; neighbors < 11; neighbors +=2)
                       {
                            for (int i = 0; i < cInfo.Length; ++i)
                            {
                                int outScale = 1;
                                Mat resized = Util.ResizeImage(image, 32 * (int)Math.Pow(2, factor), 32 * (int)Math.Pow(2, factor), out outScale);
                                cInfo[i].Scale = scale;
                                cInfo[i].MinNeighbors = neighbors;
                                Rect[] detections = FaceDetect.RunDetection(resized, cInfo[i]);
                                classifierDetections[factor - 1][i] += detections.Length;
                                detections = Util.CvtRects(detections, outScale);
                                Mat toSave = image.Clone();
                                foreach (Rect detection in detections)
                                {
                                    toSave.Rectangle(detection, Scalar.Red, 10);
                                }
                                string path = RESULT_BASE_PATH + cInfo[i].Name + "/factor" + factor + "/scale" + scale + "/neighbors" + neighbors + "/";
                                Directory.CreateDirectory(path);
                                toSave.SaveImage(path + Path.GetFileName(imagePath) + ".jpg");
                                toSave.Dispose();
                                toSave = null;
                                resized.Dispose();
                                resized = null;
                                outScale = 1;
                                GC.Collect();
                            }
                       }
                    }
                
                }
                image.Dispose();
                image = null;
                GC.Collect();
            }            
         
            Console.ReadLine();
            
        }
    }
}
