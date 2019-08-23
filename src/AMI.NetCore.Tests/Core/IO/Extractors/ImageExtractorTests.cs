using System.Collections.Generic;
using System.Threading;
using AMI.Core.Entities.Results.Commands.ProcessPath;
using AMI.Core.IO.Extractors;
using AMI.Domain.Enums;
using NUnit.Framework;

namespace AMI.NetCore.Tests.Core.IO.Extractors
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
                AmountPerAxis = 6,
                AxisTypes = new HashSet<AxisType>
                {
                    AxisType.X,
                    AxisType.Y,
                    AxisType.Z
                }
            };

            try
            {
                // Act
                var result = extractor.ProcessAsync(command, ct).Result;

                // Assert
                // 6 (per axis) * 3 (x, y, z) = 18
                Assert.AreEqual(18, result.Images.Count);
                Assert.AreEqual(5, (int)result.LabelCount);
                Assert.AreEqual(160, result.Size[0]);
                Assert.AreEqual(216, result.Size[1]);
                Assert.AreEqual(176, result.Size[2]);
            }
            finally
            {
                DeleteDirectory(command.DestinationPath);
            }
        }
    }
}
