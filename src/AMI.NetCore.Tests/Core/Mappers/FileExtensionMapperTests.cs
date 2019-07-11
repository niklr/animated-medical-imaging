using AMI.Core.Mappers;
using AMI.Domain.Enums;
using NUnit.Framework;

namespace AMI.NetCore.Tests.Core.Mappers
{
    [TestFixture]
    public class FileExtensionMapperTests : BaseTest
    {
        [TestCase("test", ".dcm", FileExtensionType.Default, FileFormat.Dicom, false)]
        [TestCase("test.test", ".dcm", FileExtensionType.Default, FileFormat.Dicom, false)]
        [TestCase("test.dcm", ".dcm", FileExtensionType.Default, FileFormat.Dicom, false)]
        [TestCase("test.hdr", ".hdr", FileExtensionType.Header, FileFormat.Analyze, false)]
        [TestCase("test.img", ".img", FileExtensionType.Source, FileFormat.Analyze, false)]
        [TestCase("test.mhd", ".mhd", FileExtensionType.Header, FileFormat.MetaImage, false)]
        [TestCase("test.raw", ".raw", FileExtensionType.Source, FileFormat.MetaImage, false)]
        [TestCase("test.nii", ".nii", FileExtensionType.Default, FileFormat.Nifti, false)]
        [TestCase("test.nii.gz", ".gz", FileExtensionType.Default, FileFormat.GZip, true)]
        [TestCase("test.tar.gz", ".gz", FileExtensionType.Default, FileFormat.GZip, true)]
        [TestCase("test.tar", ".tar", FileExtensionType.Default, FileFormat.Tar, true)]
        public void FileExtensionMapper_Map(
            string filename, 
            string expectedExtension,
            FileExtensionType expectedFileExtensionType,
            FileFormat expectedFileFormat,
            bool expectedIsArchive)
        {
            // Arrange
            var mapper = GetService<IFileExtensionMapper>();

            // Act
            var result = mapper.Map(filename);

            // Assert
            Assert.AreEqual(expectedExtension, result.Extension);
            Assert.AreEqual(expectedFileExtensionType, result.FileExtensionType);
            Assert.AreEqual(expectedFileFormat, result.FileFormat);
            Assert.AreEqual(expectedIsArchive, result.IsArchive);
        }
    }
}
