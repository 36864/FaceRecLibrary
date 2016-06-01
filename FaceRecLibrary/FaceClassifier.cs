namespace FaceRecLibrary
{
    class FaceClassifier : ClassifierInfo
    {
        public FaceClassifier(string name)
            :base(name)
        {            
        }

        public FaceClassifier(string name, string path, int weight, double scale, int minNeighbors) : base(name)
        {
            Name = name;
            Path = path;
            Weight = weight;
            Scale = scale;
            MinNeighbors = minNeighbors;
        }
    }
}
