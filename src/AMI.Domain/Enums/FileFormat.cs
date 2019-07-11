using AMI.Domain.Attributes;

namespace AMI.Domain.Enums
{
    /// <summary>
    /// A type to describe the file format.
    /// </summary>
    public enum FileFormat
    {
        /// <summary>
        /// The file format is not known.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// The Digital Imaging and Communications in Medicine (DICOM) file format.
        /// </summary>
        [FileFormatExtension(".dcm")]
        Dicom = 10,

        /// <summary>
        /// The Analyze file format.
        /// </summary>
        [FileFormatExtension(".hdr", FileExtensionType.Header)]
        [FileFormatExtension(".img", FileExtensionType.Source)]
        Analyze = 20,

        /// <summary>
        /// The MetaImage/MetaIO file format.
        /// </summary>
        [FileFormatExtension(".mha")]
        [FileFormatExtension(".mhd", FileExtensionType.Header)]
        [FileFormatExtension(".raw", FileExtensionType.Source)]
        MetaImage = 30,

        /// <summary>
        /// The Neuroimaging Informatics Technology Initiative (NIfTI) file format.
        /// </summary>
        [FileFormatExtension(".nii")]
        Nifti = 40,

        /// <summary>
        /// The proprietary Roshal Archive file format. 
        /// Extensions: .rar, .rev, .r00, .r01
        /// </summary>
        [ArchiveFileFormat]
        [FileFormatExtension(".rar")]
        [FileFormatExtension(".rev")]
        [FileFormatExtension(".r00")]
        [FileFormatExtension(".r01")]
        Rar = 50,

        /// <summary>
        /// The ZIP archive file format. 
        /// Extensions: .zip, .zipx
        /// </summary>
        [ArchiveFileFormat]
        [FileFormatExtension(".zip")]
        [FileFormatExtension(".zipx")]
        Zip = 51,

        /// <summary>
        /// The tarball archive file format.
        /// Extensions: .tar
        /// </summary>
        [ArchiveFileFormat]
        [FileFormatExtension(".tar")]
        Tar = 52,

        /// <summary>
        /// The 7z archive file format.
        /// Extensions: .7z
        /// </summary>
        [ArchiveFileFormat]
        [FileFormatExtension(".7z")]
        SevenZip = 53,

        /// <summary>
        /// The gzip archive file format.
        /// Extensions: .gz
        /// </summary>
        [ArchiveFileFormat]
        [FileFormatExtension(".gz")]
        GZip = 54
    }
}
