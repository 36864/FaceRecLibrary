using System.Drawing;

namespace FaceRecLibrary.Types
{

    public class Detection
    {
        public Rectangle Area { get; set; }
        public double Confidence { get; set; }

        public Rectangle[] Eyes { get; set; }

        public IdentityInfo Identity { get; set; }

        public Detection(){}

        public Detection(Rectangle area, double confidence = 0, IdentityInfo identity = null)
        {
            this.Identity = identity != null ? identity: new IdentityInfo();
            this.Area = area;
            this.Confidence = confidence;
        }

        public bool Conflicts(Detection detection)
        {
            Rectangle a = this.Area;
            Rectangle b = detection.Area;

            double intersectionCriteria = 0.5;
            if ((detection.Identity?._ID != null || detection.Identity.Name != null || detection.Identity.Label != null) && (!detection.Identity?.Equals(this.Identity) ?? false))
                return false;

            if (a.IntersectsWith(b) || a.Contains(b) || b.Contains(a))
            {
                Rectangle intersection = new Rectangle(a.Location, a.Size);
                intersection.Intersect(b);
                if (intersection.Width >= a.Width * intersectionCriteria || intersection.Width >= b.Width * intersectionCriteria
                    && intersection.Height >= a.Height * intersectionCriteria || intersection.Height >= b.Height * intersectionCriteria)
                    return true;
            }

            return false;
        }

        public void Merge(Detection detection)
        {
            if (detection.Confidence > this.Confidence)
            {
                this.Area = detection.Area;
                this.Eyes = detection.Eyes;
            }
        }
    }
}
