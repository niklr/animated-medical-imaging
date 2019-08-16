using System;
using System.Threading;
using AMI.Core.Entities.Results.Commands.ProcessPath;
using AMI.Core.IO.Extractors;
using AMI.Core.Strategies;
using AMI.Itk.Extractors;
using AMI.Itk.Factories;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace AMI.NetFramework.Tests.Core.IO.Extractors
{
    [TestFixture]
    public class ImageExtractorTests : BaseTest
    {
        [Test]
        public void ImageExtractor_ProcessAsync()
        {
            // Arrange
            var loggerFactory = base.GetService<ILoggerFactory>();
            var fileSystemStrategy = base.GetService<IFileSystemStrategy>();
            var factory = base.GetService<IItkImageReaderFactory>();
            var command = new ProcessPathCommand()
            {
                SourcePath = GetDataPath("SMIR.Brain.XX.O.CT.339203.nii"),
                DestinationPath = GetTempPath(),
                AmountPerAxis = 10
            };

            IImageExtractor extractor = new ItkImageExtractor(loggerFactory, fileSystemStrategy, factory);
            var ct = new CancellationToken();

            try
            {
                // Act
                var result = extractor.ProcessAsync(command, ct).Result;

                // Assert
                Assert.AreEqual(28, result.Images.Count);
            }
            finally
            {
                DeleteDirectory(command.DestinationPath);
            }
        }
    }
}
