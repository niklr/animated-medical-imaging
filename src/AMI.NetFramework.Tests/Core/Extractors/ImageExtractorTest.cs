using System;
using System.Threading;
using AMI.Core.Extractors;
using AMI.Core.Models;
using AMI.Core.Strategies;
using AMI.Itk.Extractors;
using AMI.Itk.Factories;
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
            var factory = base.GetService<IItkImageReaderFactory>();
            var input = new ExtractInput()
            {
                SourcePath = GetDataPath("SMIR.Brain.XX.O.CT.339203.nii"),
                DestinationPath = GetTempPath(),
                AmountPerAxis = 10
            };

            IImageExtractor extractor = new ItkImageExtractor(loggerFactory, fileSystemStrategy, factory);
            var ct = new CancellationToken();

            // Act
            var output = extractor.ExtractAsync(input, ct).Result;

            // Assert
            Assert.AreEqual(3*Convert.ToInt32(input.AmountPerAxis), output.Images.Count);
        }
    }
}
