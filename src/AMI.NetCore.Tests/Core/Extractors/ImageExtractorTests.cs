using System.Threading;
using AMI.Core.Entities.Paths.Commands.Process;
using AMI.Core.Extractors;
using NUnit.Framework;

namespace AMI.NetCore.Tests.Core.Extractors
{
    [TestFixture]
    public class ImageExtractorTests : BaseTest
    {
        [Test]
        public void ImageExtractor_ProcessAsync()
        {
            // Arrange
            var extractor = GetService<IImageExtractor>();
            var ct = new CancellationToken();
            var command = new ProcessPathCommand()
            {
                SourcePath = GetDataPath("SMIR.Brain_3more.XX.XX.OT.6560.mha"),
                DestinationPath = GetTempPath(),
                AmountPerAxis = 6
            };

            try
            {
                // Act
                var result = extractor.ProcessAsync(command, ct).Result;

                // Assert
                // 6 (per axis) * 3 (x, y, z) = 18
                Assert.AreEqual(18, result.Images.Count);
                Assert.AreEqual(5, (int)result.LabelCount);
            }
            finally
            {
                DeleteDirectory(command.DestinationPath);
            }
        }
    }
}
