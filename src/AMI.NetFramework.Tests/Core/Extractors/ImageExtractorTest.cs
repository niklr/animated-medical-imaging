using System;
using System.Threading;
using AMI.Core.Extractors;
using AMI.Core.Strategies;
using AMI.Itk.Extractors;
using AMI.Itk.Readers;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace AMI.NetFramework.Tests.Core.Extractors
{
    [TestFixture]
    public class ImageExtractorTest : BaseTest
    {
        [Test]
        public void NetFramework_ImageExtractor_Extract()
        {
            // Arrange
            var loggerFactory = base.GetService<ILoggerFactory>();
            var fileSystemStrategy = base.GetService<IFileSystemStrategy>();
            var reader = base.GetService<IItkImageReader>();

            string sourcePath = GetDataPath("SMIR.Brain.XX.O.CT.339203.nii");
            string destinationPath = GetTempPath();
            uint amount = 10;

            IImageExtractor extractor = new ItkImageExtractor(loggerFactory, fileSystemStrategy, reader);
            var ct = new CancellationToken();

            // Act
            var output = extractor.ExtractAsync(sourcePath, destinationPath, amount, ct).Result;

            // Assert
            Assert.AreEqual(3*Convert.ToInt32(amount), output.Images.Count);
        }
    }
}
