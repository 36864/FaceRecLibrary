using System.Drawing;

namespace FaceRecLibrary.Utilities
{

    public class Detection
    {
        public Rectangle Area { get; set; }
        public double Confidence { get; set; }

        public Rectangle[] Eyes { get; set; }

        public IdentityInfo Identity { get; set; }

        public Detection() { }

        public Detection(double confidence, Rectangle area)
        {
            this.Area = area;
            this.Confidence = confidence;
        }
    }
}
