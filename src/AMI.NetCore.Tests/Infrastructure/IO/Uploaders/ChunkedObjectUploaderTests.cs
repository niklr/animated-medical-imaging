using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.IO.Uploaders;
using AMI.NetCore.Tests.Helpers;
using NUnit.Framework;

namespace AMI.NetCore.Tests.Infrastructure.IO.Uploaders
{
    [TestFixture]
    public class ChunkedObjectUploaderTests : BaseTest
    {
        [Test]
        public void ChunkedObjectUploader_UploadAsync_1()
        {
            // Arrange
            var ct = new CancellationToken();
            string filename = "SMIR.Brain_3more.XX.XX.OT.6560.mha";
            var dataPath = GetDataPath(filename);
            var uploader = GetService<IChunkedObjectUploader>();

            // Act
            var result = UploadHelper.UploadAsync(uploader, dataPath, ct).Result;
            var fullSourcePath = GetWorkingDirectoryPath(result.SourcePath);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Id);
            Assert.AreEqual(filename, result.OriginalFilename);
            Assert.IsTrue(File.Exists(dataPath));
            Assert.IsTrue(File.Exists(fullSourcePath));
            Assert.AreEqual(new FileInfo(dataPath).Length, new FileInfo(fullSourcePath).Length);

            DeleteObject(result.Id);
            Assert.IsFalse(File.Exists(fullSourcePath));
        }

        [Test]
        public void ChunkedObjectUploader_UploadAsync_2()
        {
            // Arrange
            var ct = new CancellationToken();
            string filename = "SMIR.Brain.XX.O.CT.346124.nii";
            var dataPath = GetDataPath(filename);
            var uploader = GetService<IChunkedObjectUploader>();

            // Act
            var result = UploadHelper.UploadAsync(uploader, dataPath, ct).Result;
            var fullSourcePath = GetWorkingDirectoryPath(result.SourcePath);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Id);
            Assert.AreEqual(filename, result.OriginalFilename);
            Assert.IsTrue(File.Exists(dataPath));
            Assert.IsTrue(File.Exists(fullSourcePath));
            Assert.AreEqual(new FileInfo(dataPath).Length, new FileInfo(fullSourcePath).Length);

            DeleteObject(result.Id);
            Assert.IsFalse(File.Exists(fullSourcePath));
        }
    }

    [TestFixture]
    public class ChunkedObjectUploaderExceptionTests : BaseTest
    {
        [Test]
        public void ChunkedObjectUploader_ExceedsFileSizeLimit()
        {
            // Arrange
            var ct = new CancellationToken();
            string filename = "SMIR.Brain.XX.O.CT.346124.nii";
            var dataPath = GetDataPath(filename);
            OverrideAppOptions(new Dictionary<string, string>()
            {
                { "MaxSizeKilobytes", "1000" }
            });
            var uploader = GetService<IChunkedObjectUploader>();

            // Act
            async Task func() => await UploadHelper.UploadAsync(uploader, dataPath, ct);

            // Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(func);
            Assert.IsNotNull(ex);
            Assert.AreEqual("The file size exceeds the limit of 1000 kilobytes.", ex.Message);
        }
    }
}
