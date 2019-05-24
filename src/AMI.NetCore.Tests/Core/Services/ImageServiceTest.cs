﻿using System.IO;
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

            try
            {
                // Act
                var output = service.ExtractAsync(input, ct).Result;
                var json = File.ReadAllText(Path.Combine(input.DestinationPath, output.JsonFilename));

                // Assert
                Assert.AreEqual(input.AmountPerAxis, output.Images.Count);
                Assert.AreEqual(5, output.LabelCount);
                Assert.AreEqual(json, @"{
  ""version"": ""0.0.2.0"",
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
  ""jsonFilename"": ""output.json""
}");
            }
            finally
            {
                DeleteDirectory(input.DestinationPath);
            }
        }
    }
}
