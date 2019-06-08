namespace AMI.Domain.Enums
{
    /// <summary>
    /// A type to describe the data.
    /// </summary>
    public enum DataType
    {
        /// <summary>
        /// The type of the data is not known.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// The raw image data type.
        /// </summary>
        RawImage = 1,

        /// <summary>
        /// The segmentation image data type.
        /// </summary>
        SegmentationImage = 2
    }
}
