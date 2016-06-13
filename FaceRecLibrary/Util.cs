using OpenCvSharp.CPlusPlus;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace FaceRecLibrary
{
    public class Util
    {
        const int DEFAULT_MAX_IMAGE_WIDTH = 1024;
        const int DEFAULT_MAX_IMAGE_HEIGHT = 1024;


        public static string[] Read_List(string list)
        {
            List<string> result = new List<string>();
            string line;
            StreamReader sr = new StreamReader(list);
            while ((line = sr.ReadLine()) != null)
            {
                result.Add(line);
            }
            sr.Close();
            return result.ToArray();
        }

        public static Mat ResizeMat(Mat img, double scale)
        {
            return img.Clone().Resize(OpenCvSharp.CPlusPlus.Size.Zero, scale, scale);
        }

        public static Mat ResizeMat(Mat img, int maxWidth, int maxHeight, out double img_scale)
        {
            Mat retVal = img.Clone();
            img_scale = 1;
            if (img.Height <= maxHeight && img.Width <= maxWidth)
                return retVal;
            img_scale = FindScale(retVal.Width, retVal.Height, maxWidth, maxHeight);
            retVal = retVal.Resize(OpenCvSharp.CPlusPlus.Size.Zero, img_scale, img_scale);
            
            return retVal;
        }

        public static Rect[] CvtRects(Rect[] rects, double scale)
        {
            Rect[] retVal = new Rect[rects.Length];
            for(int i = 0; i < rects.Length; ++i)
            {
                retVal[i] = new Rect((int) (rects[i].X * scale), (int)(rects[i].Y * scale), (int)(rects[i].Width * scale), (int)(rects[i].Height * scale));
            }
            return retVal;
        }
        public static IEnumerable<List<string>> LoadAllSupportedFiles(string root, bool includeSubFolders)
        {
            string[] extensions = { ".jpg", ".gif",".png",".bmp",".jpe",".jpeg" };

            if (!includeSubFolders)
                yield return Directory.GetFiles(root, "*").Where(s => extensions.Contains(Path.GetExtension(s).ToLower())).ToList(); 
            else
            {
                Stack<string> dirs = new Stack<string>();
                dirs.Push(root);
                while (dirs.Count > 0)
                {
                    root = dirs.Pop();
                    foreach (var dir in Directory.GetDirectories(root))
                        dirs.Push(dir);
                    yield return Directory.GetFiles(root, "*").Where(s => extensions.Contains(Path.GetExtension(s).ToLower())).ToList();
                }
            }

        }


        public static List<string> DetectImages(string path, bool includeSubs)
        {
            string[] extensions = { ".jpg,*.gif,*.png,*.bmp,*.jpe,*.jpeg" };
            List<string> imgsFiles = new List<string>();
            if (Directory.Exists(path))
            {
                imgsFiles = Directory.GetFiles(path, "*", (includeSubs == true) ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly)
                            .Where(s => extensions.Contains(Path.GetExtension(s).ToLower())).ToList();
            }
            else
                imgsFiles.Add(path);
            return imgsFiles;
        }

        internal static Mat LoadImageForDetection(string imgPath, double scale)
        {
            Mat img = new Mat(imgPath, OpenCvSharp.LoadMode.GrayScale);
            return ResizeMat(img, scale);
        }

        public static string FormatImage(string imgPath, string destination, int maxHeight = -1, int maxWidth = -1)
        {
            Bitmap img = (Bitmap)Image.FromFile(imgPath);
            int height, width;

            if (maxHeight > -1)
                height = Math.Min(img.Height, maxHeight);
            else
                height = img.Height;
            if (maxWidth > -1)
                width = Math.Min(img.Width, maxWidth);
            else
                width = img.Width;
            
            destination = destination + imgPath;
            int x = -1;
            while (File.Exists(destination + "(" + x++ + ")"))
                destination = destination + "(" + x + ")";

            ChangeResolution(img, width, height, destination);
            return destination;
        }

        public static Rectangle ScaleRectangle(Rectangle rectangle, double scaleFactor)
        {
            if (scaleFactor == 1.0) return rectangle;
            return new Rectangle((int) (rectangle.X * scaleFactor), (int)(rectangle.Y * scaleFactor), (int)(rectangle.Width * scaleFactor), (int)(rectangle.Height * scaleFactor));
        }

        public static void ScaleRectangles(List<DetectionInfo.Detection> detections, double scaleFactor)
        {
            if (scaleFactor == 1.0) return;
            foreach (var detection in detections)
            {
                detection.Area = new Rectangle((int)(detection.Area.X * scaleFactor), (int)(detection.Area.Y * scaleFactor), (int)(detection.Area.Width * scaleFactor), (int)(detection.Area.Height * scaleFactor));
            }
        }


        public static Rectangle CvtRectToRectangle(Rect rect)
        {
            return new Rectangle(rect.X, rect.Y, rect.Width, rect.Height);
        }

        public static Rectangle[] CvtRectToRectangle(Rect[] rect)
        {
            Rectangle[] rectangles = new Rectangle[rect.Length];
            for (int i = 0; i < rect.Length; ++i)
                rectangles[i] = CvtRectToRectangle(rect[i]);
            return rectangles;
        }

        internal static Mat LoadImageForDetection(ImageInfo imgInfo, FaceClassifier classifier, out double img_scale )
        {        
                Mat img = new Mat(imgInfo.Path, OpenCvSharp.LoadMode.GrayScale);
                imgInfo.Width = img.Width;
                imgInfo.Height = img.Height;
                if (classifier == null)
                    img_scale = 1;
                else
                    img_scale = CalcFacialDetectionScale(imgInfo, classifier);
                return img.Resize(OpenCvSharp.CPlusPlus.Size.Zero, img_scale, img_scale);
        }

        //Formatar a imagem da path dada desta para uma resolução maxima do ecra do utilizador e consequentemente para um tipo permitido e suportado estilo jpeg e guardar na path destino dada por parâmetro
        public static IEnumerable<string> FormatImages(List<string> imgsFiles, string pathdestiny)
        {
            int screenWidth = Screen.PrimaryScreen.Bounds.Width;
            int screenHeight = Screen.PrimaryScreen.Bounds.Height;
            foreach (var imgFile in imgsFiles)
            {
                Bitmap img = (Bitmap)Image.FromFile(imgFile);
                if (img.Width > screenWidth && img.Height > screenHeight)
                {
                    string destination = pathdestiny + Path.GetFileName(imgFile);
                    int x = -1;
                    while (File.Exists(destination+"("+x+++")"))
                        destination = destination + "(" + x + ")";

                    ChangeResolution(img, screenWidth, screenHeight, destination);

                    yield return destination;
                    
                }
                else
                    yield return imgFile;
            }
        }

        public static double ChangeResolution(Bitmap img, int width, int height, string savePath)
        {
            int originWidth = img.Width;
            int originHeight = img.Height;
            float ratioA = (float)width / originWidth;
            float ratioB = (float)height / originHeight;

            //Calculates the aspect ratio of the image with the new values to keep the best quality
            float aspectRatio = Math.Min(ratioA, ratioB);

            int finalWidth = (int)(originWidth * aspectRatio);
            int finalHeight = (int)(originHeight * aspectRatio);

            Bitmap newImg = new Bitmap(finalWidth, finalHeight, PixelFormat.Format24bppRgb);

            //Improve and maintain the best quality for the image using System.Drawing.Graphics
            using (Graphics imgGraphics = Graphics.FromImage(newImg))
            {
                imgGraphics.CompositingQuality = CompositingQuality.HighQuality;
                imgGraphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                imgGraphics.SmoothingMode = SmoothingMode.HighQuality;
                imgGraphics.DrawImage(img, 0, 0, finalWidth, finalHeight);
            }

            //Initializes a object which represents the JPEG code.
            ImageCodecInfo codecInfo = GetEncoderInfo(ImageFormat.Jpeg);

            //Enconder object for quality parameter
            Encoder imgEncoder = Encoder.Quality;
            //Sets the image quality to the max
            int imgQuality = 100;
            //Creates EnconderParameters object to hold the new quality level
            EncoderParameters encoderParameters = new EncoderParameters(1);
            //Receives the new quality level of a JPEG file
            encoderParameters.Param[0] = new EncoderParameter(imgEncoder, imgQuality);
            //Saves the new image with new quality level and new format
            img.Dispose();
            GC.Collect();
            Directory.CreateDirectory(Path.GetDirectoryName(savePath));
            newImg.Save(savePath, codecInfo, encoderParameters);
            return aspectRatio;
        }

        public static Rect CvtRectangletoRect(Rectangle rectangle)
        {
            return new Rect(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
        }

        public static ImageCodecInfo GetEncoderInfo(ImageFormat imgFormat)
        {
            return ImageCodecInfo.GetImageDecoders().SingleOrDefault(codec => codec.FormatID == imgFormat.Guid);
        }

        public static RectangleF[] CvtRects(DetectionInfo dInfo, float scale, int originalWidth, int originalHeight, int newWidth, int newHeight, int offsetX = 0, int offsetY = 0)
        {
            List<DetectionInfo.Detection> detections = dInfo.Detections;
            RectangleF[] retVal = new RectangleF[detections.Count];
            scale *= FindScale(originalWidth, originalHeight, newWidth, newHeight);
            for (int i = 0; i < detections.Count; ++i)
            {
                retVal[i] = new RectangleF(offsetX + detections[i].Area.X * scale, offsetY + detections[i].Area.Y * scale, detections[i].Area.Width * scale, detections[i].Area.Height * scale);
            }
            return retVal;
        }

        public static float FindScale(int originalWidth, int originalHeight, int newWidth, int newHeight)
        {
            float scaleX = 1.0f * newWidth / originalWidth;
            float scaleY = 1.0f * newHeight / originalHeight;
            return Math.Min(scaleX, scaleY);
        }
        
        public static float CalcFacialDetectionScale(ImageInfo imgInfo, FaceClassifier classifier)
        {
            if (classifier == null)
                return FindScale(imgInfo.Width, imgInfo.Height, DEFAULT_MAX_IMAGE_WIDTH, DEFAULT_MAX_IMAGE_HEIGHT);
            else
                return FindScale(imgInfo.Width, imgInfo.Height, classifier.MaxDimensions.Width, classifier.MaxDimensions.Height);
        }

    }
}
