using FaceRecLibrary.Types;

namespace FaceRecLibrary
{
    public interface IMetadataHandler
    {
        /// <summary>
        /// Saves image detection metadata.
        /// </summary>
        /// <param name="images">Array of images of which to save metadata</param>
        /// <param name="saveParams">Parameters needed to save metadata (i.e: file paths, connection strings)</param>
        /// <returns>True if save was successful. False otherwise.</returns>
        bool Save(ImageInfo[] images, object saveParams);

        /// <summary>
        /// Saves image detection metadata.
        /// </summary>
        /// <param name="images">Image of which to save metadata</param>
        /// <param name="saveParams">Parameters needed to save metadata (i.e: file path, connection strings)</param>
        /// <returns>True if save was successful. False otherwise.</returns>
        bool Save(ImageInfo image, object saveParams);


        /// <summary>
        /// Load image metadata to the provided ImageInfo.
        /// </summary>
        /// <param name="image">Image of which to load metadata</param>
        /// <param name="loadParams">Parameters needed to load metadata (i.e: file path, connection string)</param>
        /// <returns>True if load was successful. False otherwise.</returns>
        bool Load(ImageInfo image, object loadParams);

        /// <summary>
        /// Load image metadata to the provided ImageInfo instances.
        /// </summary>
        /// <param name="image">Images of which to load metadata</param>
        /// <param name="loadParams">Parameters needed to load metadata (i.e: file path, connection string)</param>
        /// <returns>True if load was successful. False otherwise.</returns>
        bool Load(ImageInfo[] images, object loadParams);
    }
}
