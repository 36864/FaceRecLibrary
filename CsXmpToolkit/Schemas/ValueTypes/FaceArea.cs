using System;

namespace SE.Halligang.CsXmpToolkit.Schemas.ValueTypes
{
    public enum AreaType
    {
        Point,
        Circle,
        Rectangle
    }

    public enum UnitType
    {
        Pixel,
        Normalize
    }
    public class FaceArea
    {
        private XmpCore xmpCore;
        public string schemaNS, structName;
        private FaceRegionStruct faceRegionStruct;
        private AreaType type;
        private UnitType unit;
        private double? x, y, w, h, d;
        private PropertyFlags options;

        
            
		private static readonly string ns = "http://ns.adobe.com/xmp/sType/Area#";
        /// <summary>1
        /// Gets the namespace URI for the struct.
        /// </summary>
        public static string Namespace
        {
            get { return ns; }
        }

        public FaceArea(XmpCore xmpCore, string schemaNS, string structName)
        {
            this.structName = structName;
            this.xmpCore = xmpCore;
            this.schemaNS = schemaNS;
            RegisterNamespace();
        }
        public bool CheckNormalizeValues()
        {
            if (!(x >= 0.0 && x <= 1.0 && y >= 0.0 && y <= 1.0))
                return false;
            if (AreaType.Circle == type)
                if (!(d >= 0.0 && d <= 1.0))
                    return false;
            if (AreaType.Rectangle == type)
                if (!(w >= 0.0 && w <= 1.0 && h >= 0.0 && h <= 1.0))
                    return false;
            return true;
        }
        public bool CheckPixelValues()
        {
            double adX = faceRegionStruct.FaceRegion.AppliedToDimensions.Width.Value, adY = faceRegionStruct.FaceRegion.AppliedToDimensions.Height.Value;
            if (x > adX && y > adY)
                return false;
            if (AreaType.Circle == type)
                if (d > adX && d > adY)
                    return false;
            if (AreaType.Rectangle == type)
                if (w > adX && h > adY)
                    return false;
            return true;
        }
        public void ReadValues()
        {
            string fAtype = null;
            xmpCore.GetStructField(schemaNS, structName, Namespace, "type", out fAtype, out options);
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
            xmpCore.GetStructField(schemaNS, structName, Namespace, "unit", out unt, out options);
            switch (unt)
            {
                case "pixel":
                    unit = UnitType.Pixel;
                    break;
                case "normalize":
                    unit = UnitType.Normalize;
                    break;
            }

            string xValue = null, yValue = null;
            xmpCore.GetStructField(schemaNS, structName, Namespace, "x", out xValue, out options);
            xmpCore.GetStructField(schemaNS, structName, Namespace, "y", out yValue, out options);
            x = XmpUtils.ConvertToDouble(xValue);
            y = XmpUtils.ConvertToDouble(yValue);
            if (x >= 0.0 && x <= 1.0 || y >= 0.0 && y <= 1.0)
                throw new ArgumentException();

            if (Type == AreaType.Rectangle)
            {
                string wValue = null, hValue = null;
                xmpCore.GetStructField(schemaNS, structName, Namespace, "w", out wValue, out options);
                xmpCore.GetStructField(schemaNS, structName, Namespace, "h", out hValue, out options);
                w = XmpUtils.ConvertToDouble(wValue);
                h = XmpUtils.ConvertToDouble(hValue);
                if (w >= 0.0 && w <= 1.0 || h >= 0.0 && h <= 1.0)
                    throw new ArgumentException();

            }
            if (Type == AreaType.Circle)
            {
                string dValue = null;
                xmpCore.GetStructField(schemaNS, structName, Namespace, "d", out dValue, out options);
                d = XmpUtils.ConvertToDouble(dValue);
                if (d >= 0.0 && d <= 1.0)
                    throw new ArgumentException();
            }
            CheckValues();
        }
        public void SetValues(UnitType unit, AreaType type, double x, double y, double w, double h, double d)
        {
            this.unit = unit;
            this.type = type;
            this.d = d;
            this.h = h;
            this.w = w;
            this.x = x;
            this.y = y;
            CheckValues();
        }
        public void CheckValues()
        {
            if (unit == UnitType.Normalize)
                if (!CheckNormalizeValues())
                    throw new ArgumentException("An argument doesn't respect the standard values");
            if (unit == UnitType.Pixel)
                if (!CheckPixelValues())
                    throw new ArgumentException("An argument doesn't respect the standard values");
        }
        public void SetValuesToProperties()
        {
            X = x ?? -1;
            Y = y ?? -1;
            Unit = unit;
            Type = type;
            Width = w ?? -1;
            Height = h ?? -1;
            Diameter = d ?? -1;

        }
        public FaceRegionStruct FaceRegionStruct
        {
            get { return faceRegionStruct; }
            set { faceRegionStruct = value; }
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
                if (AreaType.Rectangle == type && h.HasValue)
                    return h.Value;
                throw new Exception("The required field doesn't belong to selected Area Type");
            }

            set
            {
                h = value;
                if (type == AreaType.Rectangle)
                {
                    string heightString;
                    XmpUtils.ConvertFromDouble(h.Value, null, out heightString);
                    xmpCore.SetStructField(schemaNS, structName, Namespace, "h", heightString, PropertyFlags.None);
                }
                else
                    xmpCore.DeleteStructField(schemaNS, structName, Namespace, "h");
            }
        }
        public double Width
        {
            get
            {
                if (AreaType.Rectangle == type && w.HasValue)
                    return w.Value;
                throw new Exception("The required field doesn't belong to selected Area Type");
            }

            set
            {
                w = value;
                if (type == AreaType.Rectangle)
                {
                    string widthtString;
                    XmpUtils.ConvertFromDouble(w.Value, null, out widthtString);
                    xmpCore.SetStructField(schemaNS, structName, Namespace, "w", widthtString, PropertyFlags.None);
                }
                else
                    xmpCore.DeleteStructField(schemaNS, structName, Namespace, "w");

            }
        }
        public double Diameter
        {
            get
            {
                if (AreaType.Circle == type && d.HasValue)
                    return d.Value;
                throw new Exception("The required field doesn't belong to selected Area Type");
            }

            set
            {
                d = value;
                if (type == AreaType.Circle)
                {
                    string diametertString;
                    XmpUtils.ConvertFromDouble(d.Value, null, out diametertString);
                    xmpCore.SetStructField(schemaNS, structName, Namespace, "d", diametertString, PropertyFlags.None);
                }
                else
                    xmpCore.DeleteStructField(schemaNS, structName, Namespace, "d");


            }
        }
        public double X
        {
            get
            {
                if (h.HasValue)
                    return x.Value;
                throw new Exception("The required field doesn't have a valid value");

            }

            set
            {
                x = value;
                string xString;
                XmpUtils.ConvertFromDouble(x.Value, null, out xString);
                xmpCore.SetStructField(schemaNS, structName, Namespace, "x", xString, PropertyFlags.None);
            }
        }
        public double Y
        {
            get
            {
                if (y.HasValue)
                    return y.Value;
                throw new Exception("The required field doesn't have a valid value");

            }

            set
            {
                y = value;
                string yString;
                XmpUtils.ConvertFromDouble(y.Value, null, out yString);
                xmpCore.SetStructField(schemaNS, structName, Namespace, "y", yString, PropertyFlags.None);
            }
        }
        public UnitType Unit
        {
            get
            {
                return unit;
            }
            set
            {
                unit = value;
                xmpCore.SetStructField(schemaNS, structName, Namespace, "unit", unit.ToString(), PropertyFlags.None);
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
            XmpCore.RegisterNamespace(Namespace, PreferredPrefix, out registeredPrefix);
            return registeredPrefix;
        }
    }
}
