using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using AMI.Core.Entities.Models;
using AMI.Core.Writers;
using AMI.Domain.Enums;
using NUnit.Framework;

namespace AMI.NetCore.Tests.Core.Writers
{
    [TestFixture]
    public class GifImageWriterTests : BaseTest
    {
        [Test]
        public void GifImageWriter_WriteAsync()
        {
            // Arrange
            var writer = GetService<IGifImageWriter>();
            string filename = $"combined_{Guid.NewGuid().ToString("N")}";
            string sourcePath = GetDataPath("watermark.png");
            string destinationPath = GetTempPath();
            var images = new List<PositionAxisContainerModel<string>>()
            {
                new PositionAxisContainerModel<string>(0, AxisType.X, sourcePath)
            };

            try
            {
                // Act
                var resultFilename = writer.WriteAsync(destinationPath, images, filename, BezierEasingType.Linear, new CancellationToken()).Result;

                // Assert
                string destinationFullPath = Path.Combine(destinationPath, resultFilename);
                Assert.IsTrue(File.Exists(destinationFullPath));
            }
            finally
            {
                DeleteDirectory(destinationPath);
            }
        }
    }
}
