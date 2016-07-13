using System.Drawing;
using System.Collections.Generic;

namespace FaceRecLibrary
{
    public class DetectionInfo
    {
        
        public List<Detection> Detections { get; set; }

        public DetectionInfo() {
            Detections = new List<Detection>();
        }

        public DetectionInfo(Rectangle[] areas, double confidence)
        {
            Detections = new List<Detection>();
            foreach(Rectangle area in areas)
            {
                Detections.Add(new Detection(confidence, area));
            }

        }

        public class Detection
        {
            public Rectangle Area { get; set; }
            public double Confidence { get; set; }

            public IdentityInfo Identity { get; set; }

            public Detection() { }

            public Detection(double confidence, Rectangle area)
            {
                this.Area = area;
                this.Confidence = confidence;
            }
        }
    }
}
