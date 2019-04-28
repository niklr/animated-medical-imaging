using System;
using System.Threading;
using AMI.Core.Enums;
using AMI.Core.Models;
using AMI.Core.Services;
using NUnit.Framework;

namespace AMI.NetCore.Tests.Core.Services
{
    [TestFixture]
    public class ImageServiceTest : BaseTest
    {
        [Test]
        public void NetCore_ImageService_ExtractAsync()
        {
            // Arrange
            var service = GetService<IImageService>();
            var ct = new CancellationToken();
            var input = new ExtractInput()
            {
                AmountPerAxis = 10,
                DesiredSize = 250,
                SourcePath = GetDataPath("SMIR.Brain_3more.XX.XX.OT.6560.mha"),
                DestinationPath = GetTempPath()
            };
            input.AxisTypes.Add(AxisType.Z);

            // Act
            var output = service.ExtractAsync(input, ct).Result;

            // Assert
            Assert.AreEqual(input.AmountPerAxis, output.Images.Count);
            Assert.AreEqual(5, output.LabelCount);
        }
    }
}
