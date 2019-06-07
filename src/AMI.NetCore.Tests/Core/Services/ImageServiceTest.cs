﻿using System.IO;
using System.Threading;
using AMI.Core.Entities.Models;
using AMI.Core.Entities.Objects.Commands.Process;
using AMI.Core.Services;
using AMI.Domain.Enums;
using NUnit.Framework;

namespace AMI.NetCore.Tests.Core.Services
{
    [TestFixture]
    public class ImageServiceTest : BaseTest
    {
        [Test]
        public void NetCore_ImageService_ProcessAsync()
        {
            // Arrange
            var service = GetService<IImageService>();
            var ct = new CancellationToken();
            var command = new ProcessObjectCommand()
            {
                AmountPerAxis = 10,
                DesiredSize = 250,
                SourcePath = GetDataPath("SMIR.Brain_3more.XX.XX.OT.6560.mha"),
                DestinationPath = GetTempPath()
            };
            command.AxisTypes.Add(AxisType.Z);
            var expected = @"{
  ""labelCount"": 5,
  ""images"": [
    {
      ""position"": 0,
      ""axisType"": ""z"",
      ""entity"": ""Z_0.png""
    },
    {
      ""position"": 1,
      ""axisType"": ""z"",
      ""entity"": ""Z_1.png""
    },
    {
      ""position"": 2,
      ""axisType"": ""z"",
      ""entity"": ""Z_2.png""
    },
    {
      ""position"": 3,
      ""axisType"": ""z"",
      ""entity"": ""Z_3.png""
    },
    {
      ""position"": 4,
      ""axisType"": ""z"",
      ""entity"": ""Z_4.png""
    },
    {
      ""position"": 5,
      ""axisType"": ""z"",
      ""entity"": ""Z_5.png""
    },
    {
      ""position"": 6,
      ""axisType"": ""z"",
      ""entity"": ""Z_6.png""
    },
    {
      ""position"": 7,
      ""axisType"": ""z"",
      ""entity"": ""Z_7.png""
    },
    {
      ""position"": 8,
      ""axisType"": ""z"",
      ""entity"": ""Z_8.png""
    },
    {
      ""position"": 9,
      ""axisType"": ""z"",
      ""entity"": ""Z_9.png""
    }
  ],
  ""gifs"": [
    {
      ""axisType"": ""z"",
      ""entity"": ""Z.gif""
    }
  ],
  ""combinedGif"": ""combined.gif"",
  ""version"": ""0.0.3"",
  ""jsonFilename"": ""output.json""
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
