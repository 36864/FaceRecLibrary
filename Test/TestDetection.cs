using FaceRecLibrary;
using FaceRecLibrary.Types;
using FaceRecLibrary.Utilities;
using OpenCvSharp.CPlusPlus;
using System;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace FaceRecTest
{
    class TestDetection
    {
        const string CLASSIFIER_BASE_PATH = "./Data/Classifier/";
        const string IMAGE_BASE_PATH = "./Data/Image/";
        const string RESULT_BASE_PATH = "./Results/";
        const string TEMP_DATA = "./temp/";
        const string CONFIG_FILE = "./Data/Classifier/Default_Classifiers.xml";

        public static void Main(string[] args)
        {
            string[] images = Directory.GetFiles(IMAGE_BASE_PATH, "*", SearchOption.AllDirectories).Where(
                p => p.EndsWith(".gif") || p.EndsWith(".jpg") || p.EndsWith(".png") || p.EndsWith(".jpeg")).ToArray();
            XmlReader xReader = XmlReader.Create(CONFIG_FILE);
            XmlSerializer xSerializer = new XmlSerializer(typeof(ClassifierList));
            ClassifierList cList;
            cList = (ClassifierList)xSerializer.Deserialize(xReader);
            xReader.Close();
            int detectionCount = 0;
            foreach (var imagePath in images)
            {
                string path = imagePath;
                if (imagePath.EndsWith(".gif"))
                {
                    path = Util.FormatImage(imagePath, TEMP_DATA);
                }
                ImageInfo imgInfo = new ImageInfo(path);
                DetectionInfo detections = FaceDetect.RunDetection(imgInfo, cList);
                //classifierDetections[factor - 1][i] += detections.Length;
                if (detections != null && detections.Detections.Count > 0)
                {
                    Mat toSave = new Mat(imgInfo.Path);
                    foreach (Detection detection in detections.Detections)
                    {
                        toSave.Rectangle(Util.CvtRectangletoRect(detection.Area), Scalar.Red, 10);
                    }

                    string savePath = RESULT_BASE_PATH;
                    Directory.CreateDirectory(savePath);
                    toSave.SaveImage(savePath + Path.GetFileName(imagePath) + ".jpg");

                    toSave.Dispose();
                    toSave = null;
                    GC.Collect();
                }
                detectionCount += detections.Detections.Count;
            }

            Console.Out.WriteLine("Total detections: " + detectionCount);
            Console.ReadLine();
        }

        public static void TestDetectionNoEye()
        {
            string[] images = Directory.GetFiles(IMAGE_BASE_PATH, "*", SearchOption.AllDirectories).Where(
                p => p.EndsWith(".gif") || p.EndsWith(".jpg") || p.EndsWith(".png") || p.EndsWith(".jpeg")).ToArray();
            XmlReader xReader = XmlReader.Create(CONFIG_FILE);
            XmlSerializer xSerializer = new XmlSerializer(typeof(ClassifierList));
            ClassifierList cList;
            cList = (ClassifierList)xSerializer.Deserialize(xReader);
            xReader.Close();
            cList.EyeClassifiers = null;

            int detectionCount = 0;
            foreach (var imagePath in images)
            {
                string path = imagePath;
                if (imagePath.EndsWith(".gif"))
                {
                    path = Util.FormatImage(imagePath, TEMP_DATA);
                }
                ImageInfo imgInfo = new ImageInfo(path);
                DetectionInfo detections = FaceDetect.RunDetection(imgInfo, cList);                
                if (detections != null && detections.Detections.Count > 0)
                {
                    Mat toSave = new Mat(imgInfo.Path);
                    foreach (Detection detection in detections.Detections)
                    {
                        toSave.Rectangle(Util.CvtRectangletoRect(detection.Area), Scalar.Red, 10);
                    }

                    string savePath = RESULT_BASE_PATH;
                    Directory.CreateDirectory(savePath);
                    toSave.SaveImage(savePath + Path.GetFileName(imagePath) + ".jpg");

                    toSave.Dispose();
                    toSave = null;
                    GC.Collect();
                }
                detectionCount += detections.Detections.Count;
            }

            Console.Out.WriteLine("Total detections: " + detectionCount);
            Console.ReadLine();
        }

        public static void TestClassifiers()
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
            for (int i = 0; i < 5; ++i)
            {
                classifierDetections[i] = new int[classifiers.Length];
            }


            foreach (var imagePath in images)
            {
                Mat image = new Mat(imagePath);

                for (int factor = 1; factor < 5; ++factor)
                {

                    for (double scale = 1.02; scale < 1.12; scale += 0.03)
                    {
                        for (int neighbors = 3; neighbors < 11; neighbors += 2)
                        {
                            for (int i = 0; i < cInfo.Length; ++i)
                            {
                                double outScale = 1;
                                Mat resized = Util.ResizeMat(image, 32 * (int)Math.Pow(2, factor), 32 * (int)Math.Pow(2, factor), out outScale);
                                cInfo[i].Scale = scale;
                                cInfo[i].MinNeighbors = neighbors;
                                Rect[] detections = FaceDetect.RunDetection(resized, cInfo[i]);
                                classifierDetections[factor - 1][i] += detections.Length;
                                Mat toSave = image.Clone();
                                foreach (Rect detection in detections)
                                {
                                    toSave.Rectangle(detection, Scalar.Red, 10);
                                }
                                string path = RESULT_BASE_PATH + "classifier tests/" + cInfo[i].Name + "/factor" + factor + "/scale" + scale + "/neighbors" + neighbors + "/";
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

            for(int i = 0; i < 5; ++i)
            {
                Console.Out.WriteLine(cInfo[i].Name + " detected " + i + "faces.");
            }

            Console.ReadLine();

        }
    }
}
    

    
