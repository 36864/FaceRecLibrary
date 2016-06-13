using System.Xml.Serialization;

namespace FaceRecLibrary
{
    public class ClassifierInfo
    {
        [XmlIgnore]

        const string DEFAULT_PATH = "../../Data/Classifier/";

        [XmlIgnore]
        const double DEFAULT_SCALE = 1.08;

        [XmlIgnore]
        const int DEFAULT_MIN_NEIGHBORS = 4;

        //Empty constructor for XML file loading
        public ClassifierInfo() { }

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

        [XmlIgnore]
        public string FullName
        {
            get
            {
                return Path + Name;

            }
        }

        public double Confidence { get; set; }
        
    }
}

