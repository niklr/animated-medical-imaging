using System.IO;
using System.Threading;
using AMI.Core.Entities.Results.Commands.ProcessPath;
using AMI.Core.Services;
using AMI.Domain.Enums;
using NUnit.Framework;

namespace AMI.NetCore.Tests.Infrastructure.Services
{
    [TestFixture]
    public class ImageServiceTests : BaseTest
    {
        [Test]
        public void ImageService_ProcessAsync_1()
        {
            // Arrange
            var service = GetService<IImageService>();
            var ct = new CancellationToken();
            var command = new ProcessPathCommand()
            {
                AmountPerAxis = 10,
                OutputSize = 250,
                SourcePath = GetDataPath("SMIR.Brain_3more.XX.XX.OT.6560.mha"),
                DestinationPath = GetTempPath()
            };
            command.AxisTypes.Add(AxisType.Z);
            var expected = @"{
  ""resultType"": ""ProcessResult"",
  ""labelCount"": 5,
  ""size"": [
    160,
    216,
    176
  ],
  ""images"": [
    {
      ""position"": 0,
      ""axisType"": ""Z"",
      ""entity"": ""Z_0.png""
    },
    {
      ""position"": 1,
      ""axisType"": ""Z"",
      ""entity"": ""Z_1.png""
    },
    {
      ""position"": 2,
      ""axisType"": ""Z"",
      ""entity"": ""Z_2.png""
    },
    {
      ""position"": 3,
      ""axisType"": ""Z"",
      ""entity"": ""Z_3.png""
    },
    {
      ""position"": 4,
      ""axisType"": ""Z"",
      ""entity"": ""Z_4.png""
    },
    {
      ""position"": 5,
      ""axisType"": ""Z"",
      ""entity"": ""Z_5.png""
    },
    {
      ""position"": 6,
      ""axisType"": ""Z"",
      ""entity"": ""Z_6.png""
    },
    {
      ""position"": 7,
      ""axisType"": ""Z"",
      ""entity"": ""Z_7.png""
    },
    {
      ""position"": 8,
      ""axisType"": ""Z"",
      ""entity"": ""Z_8.png""
    },
    {
      ""position"": 9,
      ""axisType"": ""Z"",
      ""entity"": ""Z_9.png""
    }
  ],
  ""gifs"": [
    {
      ""axisType"": ""Z"",
      ""entity"": ""Z.gif""
    }
  ],
  ""combinedGif"": ""combined.gif"",
  ""createdDate"": ""0001-01-01T00:00:00"",
  ""modifiedDate"": ""0001-01-01T00:00:00"",
  ""version"": ""0.0.16"",
  ""jsonFilename"": ""output.json"",
  ""id"": null,
  ""discriminator"": ""ProcessResultModel""
}";

            try
            {
                // Act
                var result = service.ProcessAsync(command, ct).Result;
                var json = File.ReadAllText(Path.Combine(command.DestinationPath, result.JsonFilename));

                // Assert
                Assert.AreEqual(command.AmountPerAxis, result.Images.Count);
                Assert.AreEqual(5, result.LabelCount);
                Assert.AreEqual(160, result.Size[0]);
                Assert.AreEqual(216, result.Size[1]);
                Assert.AreEqual(176, result.Size[2]);
                Assert.AreEqual(expected, json);
            }
            finally
            {
                DeleteDirectory(command.DestinationPath);
            }
        }

        [TestCase("SMIR.Brain.XX.O.CT.346124.dcm")]
        [TestCase("SMIR.Brain.XX.O.CT.346124.dcm.tar")]
        [TestCase("SMIR.Brain.XX.O.CT.346124.dcm.tar.gz")]
        public void ImageService_ProcessAsync_2(string filename)
        {
            // Arrange
            var service = GetService<IImageService>();
            var ct = new CancellationToken();
            var command = new ProcessPathCommand()
            {
                AmountPerAxis = 4,
                OutputSize = 50,
                SourcePath = GetDataPath(filename),
                DestinationPath = GetTempPath()
            };
            command.AxisTypes.Add(AxisType.Z);

            try
            {
                // Act
                var result = service.ProcessAsync(command, ct).Result;
                var json = File.ReadAllText(Path.Combine(command.DestinationPath, result.JsonFilename));

                // Assert
                Assert.AreEqual(command.AmountPerAxis, result.Images.Count);
            }
            finally
            {
                DeleteDirectory(command.DestinationPath);
            }
        }
    }
}
