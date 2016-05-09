using OpenCvSharp.CPlusPlus;
using System;
using System.Collections.Generic;
using System.IO;

namespace FaceRecTest
{
    class Util
    {
        public static string[] read_list(string list)
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
            img_scale = 0;
            while (img.Rows > Height && img.Cols > Width)
            {
                img_scale *= 2;
                img = img.Resize(Size.Zero, 0.5f, 0.5f);
            }
            return img;
        }
    }
}
