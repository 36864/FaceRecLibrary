using System.IO;
using FaceRecLibrary;
using OpenCvSharp.CPlusPlus;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace FaceRecTest
{
    class TestDetection
    {
        const string CLASSIFIER_BASE_PATH = "../../Data/Classifier/";
        const string IMAGE_BASE_PATH = "../../Data/Image/";
        const string RESULT_BASE_PATH = "G:/classifier tests/Results12/";
        const string TEMP_DATA = "temp/";
        const string CONFIG_FILE = "../../Data/Classifier/Default_Classifiers.xml";

        public static void Main(string[] args)
        {
            string[] images = Directory.GetFiles(IMAGE_BASE_PATH, "*", SearchOption.AllDirectories).Where(
                p => p.EndsWith(".gif") || p.EndsWith(".jpg") || p.EndsWith(".png") || p.EndsWith(".jpeg")).ToArray();
            int[][] classifierDetections = new int[5][];
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
                    foreach (DetectionInfo.Detection detection in detections.Detections)
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
    }
}

