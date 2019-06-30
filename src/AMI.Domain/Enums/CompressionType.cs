namespace AMI.Domain.Enums
{
    /// <summary>
    /// The different compression types.
    /// </summary>
    public enum CompressionType
    {
        /// <summary>
        /// No compression.
        /// </summary>
        None = 0,

        /// <summary>
        /// GZip compression.
        /// </summary>
        GZip = 1,

        /// <summary>
        /// BZip2 compression.
        /// </summary>
        BZip2 = 2,

        /// <summary>
        /// PPMd compression.
        /// </summary>
        PPMd = 3,

        /// <summary>
        /// Deflate compression.
        /// </summary>
        Deflate = 4,

        /// <summary>
        /// Rar compression.
        /// </summary>
        Rar = 5,

        /// <summary>
        /// LZMA compression.
        /// </summary>
        LZMA = 6,

        /// <summary>
        /// BCJ compression.
        /// </summary>
        BCJ = 7,

        /// <summary>
        /// BCJ2 compression.
        /// </summary>
        BCJ2 = 8,

        /// <summary>
        /// LZip compression.
        /// </summary>
        LZip = 9,

        /// <summary>
        /// Xz compression.
        /// </summary>
        Xz = 10,

        /// <summary>
        /// Unknown compression.
        /// </summary>
        Unknown = 11,

        /// <summary>
        /// Deflate64 compression.
        /// </summary>
        Deflate64 = 12
    }
}
