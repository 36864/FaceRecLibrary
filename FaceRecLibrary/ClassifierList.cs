using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceRecLibrary
{

    //To be loaded from XML files
    public class ClassifierList
    {
        private List<FaceClassifier> faceClassifiers;

        public List<FaceClassifier> FaceClassifiers
        {
            get { return faceClassifiers; }
            set { faceClassifiers = value; }
        }

        private List<EyeClassifier> eyeClassifiers;

        public List<EyeClassifier> EyeClassifiers
        {
            get { return eyeClassifiers; }
            set { eyeClassifiers = value; }
        }

        //Empty classifier for XML file loading
        public ClassifierList() { }

    }
}
