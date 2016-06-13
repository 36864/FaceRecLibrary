using System.Drawing;

namespace FaceRecLibrary
{
    public class FaceClassifier : ClassifierInfo
    {
        //Empty constructor for XML loading
        public FaceClassifier() { }

        public FaceClassifier(string name)
            :base(name)
        {            
        }

        public Size MaxDimensions { get; set; }

        public FaceClassifier(string name, string path, int weight, double scale, int minNeighbors) : base(name)
        {
            Name = name;
            Path = path;
            Confidence = weight;
            Scale = scale;
            MinNeighbors = minNeighbors;
        }
    }
}
