using SE.Halligang.CsXmpToolkit.Schemas.ValueTypes;
using System.Collections.Generic;

namespace SE.Halligang.CsXmpToolkit.Schemas.Schemas
{
    /// <summary>
    /// Region Schema
    /// </summary>
    public sealed class FaceRegionInfo
    {
        protected XmpCore xmpCore;
        public FaceRegionInfo(Xmp xmp)
        {
            RegisterNamespace();
            appliedToDimensions = new Dimensions(xmp.XmpCore, nameSpace, "AppliedToDimensions");
            
            regionList = new XmpArray<FaceRegionStruct>(xmp.XmpCore, nameSpace, "RegionList",
                 PropertyFlags.ValueIsArray,
                 new XmpArrayCallback<FaceRegionStruct>(FaceRegionStructCallback));

        }
        //(XmpCore xmpCore, string schemaNamespace, string propertyPath, PropertyFlags options, 
        //XmpArrayCallbackType type, List<T> items, int itemIndex, T itemValue
        private void FaceRegionStructCallback(XmpCore xmpCore, string schemaNS,
            string propPath, PropertyFlags options, XmpArrayCallbackType type,
            List<FaceRegionStruct> items, int itemIndex, FaceRegionStruct itemValue)
        {
            switch (type)
            {
                case XmpArrayCallbackType.Created:
                    string schema, prop, propValue;
                    XmpIterator it = new XmpIterator(xmpCore, schemaNS, propPath, IteratorMode.JustChildren);
                    while (it.Next(out schema, out prop, out propValue, out options))
                    {
                        if (prop.IndexOf('[') >= 0 && prop.IndexOf(']') >= 0)
                        {
                            //FaceArea
                            FaceArea faceA = new FaceArea(xmpCore, schemaNS, propPath);
                            string name;
                            xmpCore.GetStructField(schemaNS, prop, Namespace, "Name", out name, out options);
                            string description;
                            xmpCore.GetStructField(schemaNS, prop, Namespace, "Description", out description, out options);
                            FaceRegionStruct fRI = new FaceRegionStruct(faceA, name, description, this);
                            faceA.FaceRegionStruct = fRI;
                            items.Add(fRI);
                        }
                    }
                    break;
                case XmpArrayCallbackType.Insert:
                    for (int index = itemIndex - 1; index < items.Count; index++) { }
                    break;

                case XmpArrayCallbackType.Set:
                    itemValue.FaceRegion = null;
                    break;

                case XmpArrayCallbackType.Clear:
                    foreach (FaceRegionStruct item in items)
                    {
                        item.FaceRegion = null;
                    }
                    xmpCore.DeleteProperty(schemaNS, propPath);
                    break;

                case XmpArrayCallbackType.Removed:
                    itemValue.FaceRegion = null;
                    if (items.Count == 0)
                        xmpCore.DeleteProperty(schemaNS, propPath);
                    else
                        xmpCore.DeleteArrayItem(schemaNS, propPath, itemIndex);
                    break;

                case XmpArrayCallbackType.Added:
                    itemValue.FaceRegion = this;
                    if (!xmpCore.DoesPropertyExist(schemaNS, propPath))
                        xmpCore.SetProperty(schemaNS, propPath, null , options);
                    PropertyFlags addFlags = PropertyFlags.ValueIsStruct;
                    if (itemIndex < items.Count)
                        addFlags |= PropertyFlags.InsertBeforeItem;

                    xmpCore.SetArrayItem(schemaNS, propPath, itemIndex, null, addFlags);
                    string structPath;
                    XmpUtils.ComposeArrayItemPath(schemaNS, propPath, itemIndex, out structPath);

                    string fieldPath;
                    XmpUtils.ComposeStructFieldPath(schemaNS, structPath, Namespace, "Type", out fieldPath);
                    xmpCore.SetStructField(schemaNS, fieldPath, Namespace, "Type", itemValue.Type, PropertyFlags.None);
                    
                    if (itemValue.Name == null)
                        xmpCore.DeleteStructField(schemaNS, structPath, Namespace, "Name");
                    else
                        xmpCore.SetStructField(schemaNS, structPath, Namespace, "Name", itemValue.Name, PropertyFlags.None);
                    if (itemValue.Description == null)
                        xmpCore.DeleteStructField(schemaNS, structPath, Namespace, "Description");
                    else
                        xmpCore.SetStructField(schemaNS, structPath, Namespace, "Description", itemValue.Description, PropertyFlags.None);

   /*                 string fieldPath;
                    XmpUtils.ComposeStructFieldPath(schemaNS, structPath, Namespace, "Area", out fieldPath);
                    FaceArea fA = itemValue.Area;
                    fA.structName = fieldPath;
                    
                    FaceArea.RegisterNamespace();
                    fA.SetValuesToProperties();
                    */
                    

                    break;
            }
        }

        private static readonly string nameSpace = "http://www.metadataworkinggroup.com/schemas/regions/";

        public static string Namespace
        {
            get { return nameSpace; }
        }

        private static readonly string preferredPrefix = "mwg-rs";

        internal static string PreferredPrefix
        {
            get { return preferredPrefix; }
        }

        internal static string RegisterNamespace()
        {
            string registeredPrefix;
            XmpCore.RegisterNamespace(Namespace, PreferredPrefix, out registeredPrefix);
            return registeredPrefix;
        }

        private XmpArray<FaceRegionStruct> regionList;
        public XmpArray<FaceRegionStruct> RegionList
        {
            get { return regionList; }
        }

        private Dimensions appliedToDimensions;

        public Dimensions AppliedToDimensions
        {
            get { return appliedToDimensions; }
        }




    }
}
