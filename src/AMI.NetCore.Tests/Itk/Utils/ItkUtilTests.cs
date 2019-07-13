using AMI.Core.Mappers;
using AMI.Core.Strategies;
using AMI.Itk.Utils;
using NUnit.Framework;

namespace AMI.NetCore.Tests.Itk.Utils
{
    [TestFixture]
    public class ItkUtilTests : BaseTest
    {
        [TestCase(new string[] { }, "")]
        [TestCase(new string[] { "sample1", "sample2" }, "")]
        [TestCase(new string[] { "sample1.mhd", "sample1.raw" }, "sample1.mhd")]
        [TestCase(new string[] { @"C:\temp\sample1.mhd", @"C:\temp\sample1.raw" }, @"C:\temp\sample1.mhd")]
        [TestCase(new string[] { "sample1.raw", "sample1.mhd" }, "sample1.mhd")]
        public void ItkUtil_DiscoverEntryPath(string[] files, string expected)
        {
            // Arrange
            var strategy = GetService<IFileSystemStrategy>();
            var mapper = GetService<IFileExtensionMapper>();
            var util = new ItkUtil(strategy, mapper);

            // Act
            var result = util.DiscoverEntryPath(files);

            // Assert
            Assert.AreEqual(expected, result);
        }
    }
}
