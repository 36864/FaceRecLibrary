using System.Drawing;
using System.Collections.Generic;

namespace FaceRecLibrary.Utilities
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

    }
}
