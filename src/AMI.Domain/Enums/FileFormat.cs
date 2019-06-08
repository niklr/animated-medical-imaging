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
        Dicom = 1,

        /// <summary>
        /// The multi-frame DICOM file format.
        /// </summary>
        DicomMultiframe = 2,

        /// <summary>
        /// The Analyze file format.
        /// </summary>
        Analyze = 3,

        /// <summary>
        /// The MetaImage/MetaIO file format.
        /// </summary>
        MetaImage = 4,

        /// <summary>
        /// The Neuroimaging Informatics Technology Initiative (NIfTI) file format.
        /// </summary>
        Nifti = 5
    }
}
