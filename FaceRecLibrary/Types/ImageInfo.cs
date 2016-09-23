using System.Collections.Generic;
using System.Drawing;

namespace FaceRecLibrary.Types
{
    public class ImageInfo
    {
        public string Path { get; set; }

        public string OriginalPath { get; set; }

        public List<Detection> Detections { get; set; }

        public ImageInfo() { }

        public ImageInfo(string path)
        {
            OriginalPath = Path = path;            
            DisplayScaleFactor = 1;
        }

        public int Width { get; set; }
        public int Height { get; set; }

        //Must be set for displaying
        public double DisplayScaleFactor { get; set; }

        [System.Xml.Serialization.XmlIgnore]
        public bool IsSaved { get; set; }

        public void AddDetections(Rectangle[] areas, double confidence, IdentityInfo[] identities = null)
        {
            if (Detections == null)
                Detections = new List<Detection>();
            for(int i = 0; i < areas.Length; ++i)
            {
                Detections.Add(new Detection(areas[i], confidence, identities?[i], this));
            }
        }

        public void AddDetection(Detection detection)
        {
            if (Detections == null)
                Detections = new List<Detection>();
            Detections.Add(detection);
        }
    }
}
