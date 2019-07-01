﻿using System.IO;
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
        public void ImageService_ProcessAsync()
        {
            // Arrange
            var service = GetService<IImageService>();
            var ct = new CancellationToken();
            var command = new ProcessPathCommand()
            {
                AmountPerAxis = 10,
                DesiredSize = 250,
                SourcePath = GetDataPath("SMIR.Brain_3more.XX.XX.OT.6560.mha"),
                DestinationPath = GetTempPath()
            };
            command.AxisTypes.Add(AxisType.Z);
            var expected = @"{
  ""resultType"": 1,
  ""labelCount"": 5,
  ""images"": [
    {
      ""position"": 0,
      ""axisType"": 2,
      ""entity"": ""Z_0.png""
    },
    {
      ""position"": 1,
      ""axisType"": 2,
      ""entity"": ""Z_1.png""
    },
    {
      ""position"": 2,
      ""axisType"": 2,
      ""entity"": ""Z_2.png""
    },
    {
      ""position"": 3,
      ""axisType"": 2,
      ""entity"": ""Z_3.png""
    },
    {
      ""position"": 4,
      ""axisType"": 2,
      ""entity"": ""Z_4.png""
    },
    {
      ""position"": 5,
      ""axisType"": 2,
      ""entity"": ""Z_5.png""
    },
    {
      ""position"": 6,
      ""axisType"": 2,
      ""entity"": ""Z_6.png""
    },
    {
      ""position"": 7,
      ""axisType"": 2,
      ""entity"": ""Z_7.png""
    },
    {
      ""position"": 8,
      ""axisType"": 2,
      ""entity"": ""Z_8.png""
    },
    {
      ""position"": 9,
      ""axisType"": 2,
      ""entity"": ""Z_9.png""
    }
  ],
  ""gifs"": [
    {
      ""axisType"": 2,
      ""entity"": ""Z.gif""
    }
  ],
  ""combinedGif"": ""combined.gif"",
  ""createdDate"": ""0001-01-01T00:00:00"",
  ""modifiedDate"": ""0001-01-01T00:00:00"",
  ""version"": ""0.0.3"",
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
                Assert.AreEqual(expected, json);
            }
            finally
            {
                DeleteDirectory(command.DestinationPath);
            }
        }
    }
}
