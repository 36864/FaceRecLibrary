using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FaceRecLibrary.Types;
using SE.Halligang.CsXmpToolkit.Schemas;
using SE.Halligang.CsXmpToolkit.Schemas.Schemas;
using FaceRecLibrary.Utilities;
using SE.Halligang.CsXmpToolkit.Schemas.ValueTypes;
using System.IO;

namespace FaceRecLibrary
{
    class XMPMetadataHandler : IMetadataHandler
    {
        public bool Load(ImageInfo[] images, object loadParams = null)
        {
            bool success = false;
            foreach(ImageInfo image in images)
            {
                success = Load(image);
            }
            return success;
        }

        public bool Load(ImageInfo image, object loadParams = null)
        {
            Xmp x = Xmp.FromFile(image.OriginalPath, XmpFileMode.ReadWrite);

            FaceRegionInfo fri = new FaceRegionInfo(x);

            foreach (FaceRegionStruct frs in fri.RegionList)
            {
                Detection d = new Detection();
                d.Area = new System.Drawing.Rectangle((int)frs.Area.X, (int)frs.Area.Y, (int)frs.Area.Width, (int)frs.Area.Height);
                d.Identity = new IdentityInfo();
                d.Identity.Name = frs.Name;
                if (image.DetectionInfo?.Detections == null || !image.DetectionInfo.Detections.Contains(d))
                    image.AddDetection(d);
            }

            x.Dispose();
            return true;
        }

        public bool Save(ImageInfo image, object saveParams = null)
        {
            XMPMetadataHandlerParameters xmpParams = (XMPMetadataHandlerParameters) saveParams;
            string filePath;

            if (xmpParams?.SavePath != null && !xmpParams.SavePath.Equals(image.OriginalPath))
            {
                try {
                    File.Copy(image.OriginalPath, xmpParams.SavePath, true);
                }
                catch (Exception)
                {
                    return false;
                }
                filePath = xmpParams.SavePath;
            }
            else
            {
                filePath = image.OriginalPath;
            }

            Xmp x = Xmp.FromFile(image.OriginalPath, XmpFileMode.ReadWrite);

            Util.MergeDuplicates(image.DetectionInfo);

            FaceRegionInfo fri = new FaceRegionInfo(x);
            if(xmpParams.ClearBeforeSave)
                fri.RegionList.Clear();
            fri.AppliedToDimensions.SetDimensions(image.Width, image.Height, "pixel");

            foreach (Detection d in image.DetectionInfo.Detections)
            {
                FaceArea fArea = new FaceArea(x.XmpCore, FaceRegionInfo.Namespace, "Area");
                FaceRegionStruct frs = new FaceRegionStruct(fArea, d.Identity?.Name, null, fri);
                fArea.SetValues(UnitType.Pixel, AreaType.Rectangle, d.Area.X, d.Area.Y, d.Area.Width, d.Area.Height, 0);
                fri.RegionList.Add(frs);
            }
            x.Save();
            x.Dump();
            x.Dispose();
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="images"></param>
        /// <param name="saveParams"></param>
        /// <returns>True if all files save successfuly. False if any fail to save or if there are no files to save.</returns>
        public bool Save(ImageInfo[] images, object saveParams = null)
        {
            bool success = false;
            XMPMetadataHandlerParameters[] xmpParams = (XMPMetadataHandlerParameters[]) saveParams;

            for(int i = 0; i < images.Length; ++i)
            {
                success = Save(images[i], xmpParams?[i]);
            }
            return success;
        }
    }
}
