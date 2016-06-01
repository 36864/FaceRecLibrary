using OpenCvSharp.CPlusPlus;
using System.Collections.Generic;

namespace FaceRecLibrary
{
    class DetectionInfo
    {
        public ClassifierInfo Classifier { get; set; }

        public List<Rect> Detections { get; set; }

    }
}
