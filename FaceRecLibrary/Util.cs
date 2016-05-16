using OpenCvSharp.CPlusPlus;
using System.Collections.Generic;
using System.IO;
using System.Drawing;

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

        public static RectangleF[] CvtRects(Rect[] detections, float scale, int baseX, int baseY)
        {
            RectangleF[] retVal = new RectangleF[detections.Length];
            for(int i = 0; i < detections.Length; ++i)
            {
                retVal[i] = new RectangleF( baseX + detections[i].X * scale, baseY + detections[i].Y * scale, detections[i].Width * scale, detections[i].Height * scale);
            }
            return retVal;
        }
    }
}
