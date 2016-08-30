using SE.Halligang.CsXmpToolkit.Schemas.Schemas;
using System.Reflection;

namespace SE.Halligang.CsXmpToolkit.Schemas.ValueTypes
{
    public class FaceRegionStruct
    {
        public FaceRegionStruct(FaceArea area, string name, string description, FaceRegionInfo FaceRegion)
        {
            this.area = area;
            this.name = name;
            this.description = description;
            faceRegion = FaceRegion;
            area.FaceRegionStruct = this;
        }
        private FaceRegionInfo faceRegion;
        public FaceRegionInfo FaceRegion
        {
            get { return faceRegion; }
            set { faceRegion = value; }
        }

        private FaceArea area;
        public FaceArea Area
        {
            //Duvida de se devo verificar se a propriedade deveria estar a null ou nao
            get { return area; }
            set { area = value; }
        }

        private static readonly string type = "Face";

        public string Type
        {
            get { return type; }
        }

        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private string description;

        public void setValueToProperty(XmpCore xmpCore, FaceRegionStruct f, string prop, string schemaNS, string structPath, string Namespace)
        {
            PropertyInfo p = f.GetType().GetProperty(prop);

            if(p.GetValue(f, null) == null)
                xmpCore.DeleteStructField(schemaNS, structPath, Namespace, "Description");
            else
                xmpCore.SetStructField(schemaNS, structPath, Namespace, "Description", p.GetValue(f, null).ToString(), PropertyFlags.None);
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }


    }
}
