using System.Threading;
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
            string sourcePath = GetDataPath("SMIR.Brain_3more.XX.XX.OT.6560.mha");
            string destinationPath = GetTempPath();
            uint amount = 6;

            // Act
            var output = extractor.ExtractAsync(sourcePath, destinationPath, amount, ct).Result;

            // Assert
            // 6 (per axis) * 3 (x, y, z) = 18
            Assert.AreEqual(18, output.Images.Count);
            Assert.AreEqual(5, (int)output.LabelCount);
        }
    }
}
