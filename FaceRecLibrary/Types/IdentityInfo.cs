using FaceRecLibrary.Utilities;
using OpenCvSharp.CPlusPlus;
using System;
using System.Collections.Generic;

namespace FaceRecLibrary.Types
{
    public class IdentityInfo
    {
        public IdentityInfo(string name, int? label = null)
        {
            Name = name;
            Label = label;
            Detections = new List<Detection>();
        }

        public IdentityInfo()
        {
            Detections = new List<Detection>();
        }

        public string Name { get; set; }

        public int ?Label { get; set; }

        public List<Detection> Detections { get; set; }

        public bool Equals(IdentityInfo identity)
        {
            if (identity.Label == null && this.Label == null)
                if (identity.Name?.Equals(this.Name) != null)
                    return true;
                else
                    return false;
            if (identity.Label == this.Label) {
                if (this.Name == null)
                    this.Name = identity.Name;
                return true;
            }
            return false;
        }

        public Mat[] GetFaceImages(Size faceDimensions)
        {
            Mat[] faces = new Mat[Detections.Count];
            for (int i = 0; i < Detections.Count; i++)
            {
                try {
                    using (Mat src = new Mat(Detections[i].Image.OriginalPath, OpenCvSharp.LoadMode.GrayScale))
                    {
                        faces[i] = src.SubMat(Util.CvtRectangletoRect(Detections[i].Area)).Resize(faceDimensions);
                    }
                }
                catch (Exception)
                {
                    faces[i] = null;
                }
            }

            return faces;
        }

        internal void AddDetection(Detection detection)
        {
            Detections.Add(detection);
        }
    }
}