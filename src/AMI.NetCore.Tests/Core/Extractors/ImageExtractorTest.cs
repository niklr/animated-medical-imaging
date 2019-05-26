using System.Threading;
using AMI.Core.Entities.Objects.Commands.Extract;
using AMI.Core.Extractors;
using NUnit.Framework;

namespace AMI.NetCore.Tests.Core.Extractors
{
    [TestFixture]
    public class ImageExtractorTest : BaseTest
    {
        [Test]
        public void NetCore_ImageExtractor_ExtractAsync()
        {
            // Arrange
            var extractor = GetService<IImageExtractor>();
            var ct = new CancellationToken();
            var command = new ExtractObjectCommand()
            {
                SourcePath = GetDataPath("SMIR.Brain_3more.XX.XX.OT.6560.mha"),
                DestinationPath = GetTempPath(),
                AmountPerAxis = 6
            };

            try
            {
                // Act
                var result = extractor.ExtractAsync(command, ct).Result;

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
