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

            return result.ToArray();
        }
    }
}
