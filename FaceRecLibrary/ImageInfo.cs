using OpenCvSharp.CPlusPlus;

namespace FaceRecLibrary
{
    public class ImageInfo
    {
        public string Path { get; set; }

        public int Scale;

        public Rect[][] Detections { get; set; }

        public ImageInfo() { }

        public ImageInfo(string path)
        {
            Path = path;
            Scale = 1;
            Detections = null;
        }

        [System.Xml.Serialization.XmlIgnore]
        public bool IsSaved { get; set; }
    }
}
