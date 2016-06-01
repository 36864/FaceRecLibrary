namespace FaceRecLibrary
{
    class EyeClassifier : ClassifierInfo
    {
        public EyeClassifier(string name)
            :base(name)
        {            
        }

        public EyeClassifier(string name, string path, int weight, double scale, int minNeighbors) : base(name)
        {
            Name = name;
            Path = path;
            Weight = weight;
            Scale = scale;
            MinNeighbors = minNeighbors;
        }
    }
}
