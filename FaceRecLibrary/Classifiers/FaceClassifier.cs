using System.Drawing;

namespace FaceRecLibrary
{
    public class FaceClassifier : ClassifierInfo
    {
        //Empty constructor for XML loading
        public FaceClassifier() { }

        public FaceClassifier(string name) 
            : base(name)
        {
        }

        private Size maxDimensions;
        public Size MaxDimensions
        {
            get { return maxDimensions; }
            set { maxDimensions = value; }
        }

        public FaceClassifier(string name, string path, int weight, double scale, int minNeighbors, Size? maxDimensions = null)
        {
            Name = name;
            Path = path;
            Confidence = weight;
            Scale = scale;
            MinNeighbors = minNeighbors;
            MaxDimensions = maxDimensions.Value;
        }
    }
}
