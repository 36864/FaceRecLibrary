using OpenCvSharp.CPlusPlus;
using System;
using System.Collections.Generic;
using System.IO;

namespace FaceRecTest
{
    class Util
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

        public static Mat ResizeImage(Mat img, int height, int width, out int img_scale)
        {
            img_scale = 0;
            while (img.Rows > height && img.Cols > width)
            {
                img_scale *= 2;
                img = img.Resize(Size.Zero, 0.5f, 0.5f);
            }
            return img;
        }
    }
}
