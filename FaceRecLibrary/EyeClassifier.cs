namespace FaceRecLibrary
{
    public class EyeClassifier : ClassifierInfo
    {
        //Parameterless constructor for XML Serialization
        public EyeClassifier() { }


        public EyeClassifier(string name)
            :base(name)
        {            
        }

        public EyeClassifier(string name, string path, int weight, double scale, int minNeighbors) : base(name)
        {
            Name = name;
            Path = path;
            Confidence = weight;
            Scale = scale;
            MinNeighbors = minNeighbors;
        }
    }
}
