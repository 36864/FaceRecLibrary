using OpenCvSharp.CPlusPlus;
using System.Collections.Generic;
using System.IO;
using System.Drawing;
using System;
using System.Windows.Forms;
using System.Linq;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace FaceRecLibrary
{
    public class Util
    {
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

        public static Mat ResizeImage(Mat img, int Height, int Width, out int img_scale)
        {
            img_scale = 1;
            while (img.Rows > Height && img.Cols > Width)
            {
                img_scale *= 2;
                img = img.Resize(OpenCvSharp.CPlusPlus.Size.Zero, 0.5f, 0.5f);
            }
            return img;
        }

        //Formatar a imagem da path dada desta para uma resolução maxima do ecra do utilizador e consequentemente para um tipo permitido e suportado estilo jpeg e guardar na path destino dada por parâmetro
        public static void FormatImage(string path, string pathdestiny)
        {
            int screenWidth = Screen.PrimaryScreen.Bounds.Width;
            int screenHeight = Screen.PrimaryScreen.Bounds.Height;
            string extensions = "*.jpg,*.gif,*.png,*.bmp,*.jpe,*.jpeg";
            if (File.Exists(path))
            {
                foreach (var imgFile in Directory.GetFiles("@" + path, "*", SearchOption.AllDirectories)
                            .Where(s => extensions.Contains(Path.GetExtension(s).ToLower())))
                {
                    ChangeResolution((Bitmap)Image.FromFile(imgFile), screenWidth, screenHeight, pathdestiny);
                }
            }
        }


        public static void ChangeResolution(Bitmap img, int width, int height, string savePath)
        {
            int originWidth = img.Width, originHeight = img.Height;
            float ratioA = (float)originWidth / width;
            float ratioB = (float)originHeight / height;

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
            newImg.Save(savePath, codecInfo, encoderParameters);
        }

        public static ImageCodecInfo GetEncoderInfo(ImageFormat imgFormat)
        {
            return ImageCodecInfo.GetImageDecoders().SingleOrDefault(codec => codec.FormatID == imgFormat.Guid);
        }

        public static RectangleF[] CvtRects(Rect[] detections, float scale, int originalWidth, int originalHeight, int newWidth, int newHeight, int offsetX = 0, int offsetY = 0)
        {
            RectangleF[] retVal = new RectangleF[detections.Length];
            float scaleX = 1.0f * originalWidth / newWidth;
            float scaleY = 1.0f * newHeight / originalHeight;
            scale *= Math.Min(scaleX, scaleY);
            for (int i = 0; i < detections.Length; ++i)
            {
                retVal[i] = new RectangleF(offsetX + detections[i].X * scale, offsetY + detections[i].Y * scale, detections[i].Width * scale, detections[i].Height * scale);
            }
            return retVal;
        }
    }
}
