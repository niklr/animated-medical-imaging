using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using AMI.Core.Enums;
using AMI.Core.Models;
using AMI.Core.Writers;
using NUnit.Framework;

namespace AMI.NetCore.Tests.Core.Writers
{
    [TestFixture]
    public class GifImageWriterTest : BaseTest
    {
        [Test]
        public void NetCore_GifImageWriter_WriteAsync()
        {
            // Arrange
            var writer = GetService<IGifImageWriter>();
            string filename = $"combined_{Guid.NewGuid().ToString("N")}";
            string sourcePath = GetDataPath("watermark.png");
            string destinationPath = Path.GetTempPath();
            var images = new List<PositionAxisContainer<string>>()
            {
                new PositionAxisContainer<string>(0, AxisType.X, sourcePath)
            };

            // Act
            var resultFilename = writer.WriteAsync(destinationPath, images, filename, BezierEasingType.Linear, new CancellationToken()).Result;

            // Assert
            string destinationFullPath = Path.Combine(destinationPath, resultFilename);
            Assert.IsTrue(File.Exists(destinationFullPath));
            DeleteFile(destinationFullPath);
        }
    }
}
