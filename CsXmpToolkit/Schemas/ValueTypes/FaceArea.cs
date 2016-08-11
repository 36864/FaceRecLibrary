using SE.Halligang.CsXmpToolkit.Schemas.Schemas;
using System;

namespace SE.Halligang.CsXmpToolkit.Schemas.ValueTypes
{
    public enum AreaType
    {
        Point,
        Circle,
        Rectangle
    }
    public class FaceArea
    {
        private XmpCore xmpCore;
        private string schemaNS, unit, structName;
        private FaceRegionStruct faceRegionInfo;
        private AreaType type;
        private double x, y, w, h, d;
        private PropertyFlags options;

        public FaceArea(XmpCore xmpCore, string schemaNS, string structName)
        {
            this.structName = structName;
            this.xmpCore = xmpCore;
            this.schemaNS = schemaNS;
            RegisterNamespace();
        }

        public void ReadValues()
        {

            string fAtype = null;
            xmpCore.GetStructField(schemaNS, structName, Schemas.FaceRegionInfo.Namespace, "type", out fAtype, out options);
            switch (fAtype.ToLower())
            {
                case "point":
                    type = AreaType.Point;
                    break;
                case "circle":
                    type = AreaType.Circle;
                    break;
                case "rectangle":
                    type = AreaType.Rectangle;
                    break;
            }
            string unt = null;
            xmpCore.GetStructField(schemaNS, structName, Schemas.FaceRegionInfo.Namespace, "unit", out unt, out options);
            unit = unt;

            string xValue = null, yValue = null;
            xmpCore.GetStructField(schemaNS, structName, Schemas.FaceRegionInfo.Namespace, "x", out xValue, out options);
            xmpCore.GetStructField(schemaNS, structName, Schemas.FaceRegionInfo.Namespace, "y", out yValue, out options);
            x = XmpUtils.ConvertToDouble(xValue);
            y = XmpUtils.ConvertToDouble(yValue);
            if (x >= 0.0 && x <= 1.0 || y >= 0.0 && y <= 1.0)
                throw new ArgumentException();

            if (Type == AreaType.Rectangle)
            {
                string wValue = null, hValue = null;
                xmpCore.GetStructField(schemaNS, structName, Schemas.FaceRegionInfo.Namespace, "w", out wValue, out options);
                xmpCore.GetStructField(schemaNS, structName, Schemas.FaceRegionInfo.Namespace, "h", out hValue, out options);
                w = XmpUtils.ConvertToDouble(wValue);
                h = XmpUtils.ConvertToDouble(hValue);
                if (w >= 0.0 && w <= 1.0 || h >= 0.0 && h <= 1.0)
                    throw new ArgumentException();

            }
            if (Type == AreaType.Circle)
            {
                string dValue = null;
                xmpCore.GetStructField(schemaNS, structName, Schemas.FaceRegionInfo.Namespace, "d", out dValue, out options);
                d = XmpUtils.ConvertToDouble(dValue);
                if (d >= 0.0 && d <= 1.0)
                    throw new ArgumentException();
            }

        }

        public void SetValues(string unit, AreaType type, double x, double y, double w = 5.0, double h = 5.0, double d = 5.0)
        {
            this.unit = unit;
            this.type = type;
            this.x = x;
            this.y = y;
            if (d >= 0.0 && d <= 1.0 && type == AreaType.Circle)
                this.d = d;
            if (h >= 0.0 && h <= 1.0 && w >= 0.0 && w <= 1.0 && type == AreaType.Rectangle)
            {
                this.h = h;
                this.w = w;
            }
            ImplementValues();
        }
        public void ImplementValues()
        {
            X = x;
            Y = y;
            Unit = unit;
            Type = type;
            Width = w;
            Height = h;
            Diameter = d;

        }
        public FaceRegionStruct FaceRegionInfo
        {
            get { return faceRegionInfo; }
            set { faceRegionInfo = value; }
        }
        public AreaType Type
        {
            get { return type; }
            set { type = value; }
        }
        public double Height
        {
            get
            {
                if (AreaType.Rectangle == type)
                    return h;
                throw new Exception("The required field doesn't belong to selected Area Type");
            }

            set
            {
                h = value;
                if (type == AreaType.Rectangle)
                {
                    string heightString;
                    XmpUtils.ConvertFromDouble(h, null, out heightString);
                    xmpCore.SetStructField(schemaNS, structName, Schemas.FaceRegionInfo.Namespace, "h", heightString, PropertyFlags.None);
                }
                else
                    xmpCore.DeleteStructField(schemaNS, structName, Schemas.FaceRegionInfo.Namespace, "h");
            }
        }
        public double Width
        {
            get
            {
                if (AreaType.Rectangle == type)
                    return w;
                throw new Exception("The required field doesn't belong to selected Area Type");
            }

            set
            {
                w = value;
                if (type == AreaType.Rectangle)
                {
                    string widthtString;
                    XmpUtils.ConvertFromDouble(w, null, out widthtString);
                    xmpCore.SetStructField(schemaNS, structName, Schemas.FaceRegionInfo.Namespace, "w", widthtString, PropertyFlags.None);
                }
                else
                    xmpCore.DeleteStructField(schemaNS, structName, Schemas.FaceRegionInfo.Namespace, "w");

            }
        }
        public double Diameter
        {
            get
            {
                if (AreaType.Circle == type)
                    return d;
                throw new Exception("The required field doesn't belong to selected Area Type");
            }

            set
            {
                d = value;
                if (type == AreaType.Circle)
                {
                    string diametertString;
                    XmpUtils.ConvertFromDouble(d, null, out diametertString);
                    xmpCore.SetStructField(schemaNS, structName, Schemas.FaceRegionInfo.Namespace, "d", diametertString, PropertyFlags.None);
                }
                else
                    xmpCore.DeleteStructField(schemaNS, structName, Schemas.FaceRegionInfo.Namespace, "d");


            }
        }
        public double X
        {
            get
            {
                return x;
            }

            set
            {
                x = value;
                string xString;
                XmpUtils.ConvertFromDouble(x, null, out xString);
                xmpCore.SetStructField(schemaNS, structName, Schemas.FaceRegionInfo.Namespace, "x", xString, PropertyFlags.None);
            }
        }
        public double Y
        {
            get
            {
                return y;
            }

            set
            {
                y = value;
                string yString;
                XmpUtils.ConvertFromDouble(y, null, out yString);
                xmpCore.SetStructField(schemaNS, structName, Schemas.FaceRegionInfo.Namespace, "y", yString, PropertyFlags.None);
            }
        }
        public string Unit
        {
            get
            {
                return unit;
            }
            set
            {
                unit = value;
                xmpCore.SetStructField(schemaNS, structName, Schemas.FaceRegionInfo.Namespace, "unit", unit, PropertyFlags.None);
            }
        }

        private static readonly string prefix = "stArea";
        internal static string PreferredPrefix
        {
            get { return prefix; }
        }
        internal static string RegisterNamespace()
        {
            string registeredPrefix;
            XmpCore.RegisterNamespace(Schemas.FaceRegionInfo.Namespace, PreferredPrefix, out registeredPrefix);
            return registeredPrefix;
        }
    }
}
