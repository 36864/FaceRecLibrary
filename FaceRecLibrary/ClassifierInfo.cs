using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceRecLibrary
{
    public class ClassifierInfo
    {
        public ClassifierInfo(string path)
        {
            Path = path;
            Scale = 1.1;
            MinNeighbors = 5;
        }

        public string Path { get; set; }

        public double Scale { get; set; }

        public int MinNeighbors { get; set; }
    }
}

