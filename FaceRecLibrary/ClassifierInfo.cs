namespace FaceRecLibrary
{
    public class ClassifierInfo
    {
        private string path;
        const string DEFAULT_PATH = "../../Data/Classifier";
        public ClassifierInfo(string name)
        {
            Name = name;
            path = DEFAULT_PATH;
            Scale = 1.1;
            MinNeighbors = 5;
        }

        public string Name { get; set; }

        public string Path
        {
            get { return path; }
            set { path = value; }
        }

        public double Scale { get; set; }

        public int MinNeighbors { get; set; }

        public string FullName
        {
            get
            {
                return Path + Name;

            }
        }
    }
}

