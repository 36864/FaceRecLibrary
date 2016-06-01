namespace FaceRecLibrary
{
    public class ClassifierInfo
    {
        const string DEFAULT_PATH = "../../Data/Classifier/";
        const double DEFAULT_SCALE = 1.08;
        const int DEFAULT_MIN_NEIGHBORS = 4;

        public ClassifierInfo(string name)
        {
            Name = name;
            Path = DEFAULT_PATH;
            Scale = DEFAULT_SCALE;
            MinNeighbors = DEFAULT_MIN_NEIGHBORS;
        }

        public string Name { get; set; }

        public string Path { get; set; }

        public double Scale { get; set; }

        public int MinNeighbors { get; set; }

        public string FullName
        {
            get
            {
                return Path + Name;

            }
        }

        public int Weight { get; set; }

    }
}

