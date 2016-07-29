﻿namespace FaceRecLibrary.Utilities
{
    public class ImageInfo
    {
        public string Path { get; set; }

        public string OriginalPath { get; set; }

        public DetectionInfo DetectionInfo { get; set; }

        public ImageInfo() { }

        public ImageInfo(string path)
        {
            OriginalPath = Path = path;            
            DisplayScaleFactor = -1;
        }

        public int Width { get; set; }
        public int Height { get; set; }

        //Must be set for displaying
        public double DisplayScaleFactor { get; set; }

        [System.Xml.Serialization.XmlIgnore]
        public bool IsSaved { get; set; }

        public void AddDetection(Detection detection)
        {
            this.DetectionInfo.Detections.Add(detection);
        }
    }
}