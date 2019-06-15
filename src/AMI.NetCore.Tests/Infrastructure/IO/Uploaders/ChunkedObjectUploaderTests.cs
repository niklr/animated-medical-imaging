using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Entities.Models;
using AMI.Core.IO.Uploaders;
using NUnit.Framework;

namespace AMI.NetCore.Tests.Infrastructure.IO.Uploaders
{
    [TestFixture]
    public class ChunkedObjectUploaderTests : BaseTest
    {
        private readonly IChunkedObjectUploader uploader;

        public ChunkedObjectUploaderTests()
        {
            uploader = GetService<IChunkedObjectUploader>();
        }

        [Test]
        public void ChunkedObjectUploader_UploadAsync_1()
        {
            // Arrange
            var ct = new CancellationToken();
            string filename = "SMIR.Brain_3more.XX.XX.OT.6560.mha";
            var dataPath = GetDataPath(filename);

            // Act
            var result = UploadAsync(dataPath, ct).Result;
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

            // Act
            var result = UploadAsync(dataPath, ct).Result;
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

        private async Task<ObjectModel> UploadAsync(string dataPath, CancellationToken ct)
        {
            string uid = Guid.NewGuid().ToString();
            string filename = Path.GetFileName(dataPath);
            using (FileStream stream = new FileStream(dataPath, FileMode.Open))
            {
                long chunkLength = 1048576;
                if (stream.Length < chunkLength)
                {
                    chunkLength = stream.Length;
                }
                long totalChunks = stream.Length / chunkLength;

                byte[] chunk = new byte[chunkLength];
                int chunkNumber = 1;
                int maximumNumberOfBytesToRead = chunk.Length;
                while (stream.Read(chunk, 0, maximumNumberOfBytesToRead) > 0)
                {
                    using (MemoryStream chunkStream = new MemoryStream(chunk))
                    {
                        await uploader.UploadAsync(Convert.ToInt32(totalChunks), chunkNumber, uid, chunkStream, ct);
                    }

                    long numberOfBytesToReadLeft = stream.Length - stream.Position;
                    if (numberOfBytesToReadLeft < maximumNumberOfBytesToRead)
                    {
                        maximumNumberOfBytesToRead = (int)numberOfBytesToReadLeft;
                        chunk = new byte[maximumNumberOfBytesToRead];
                    }
                    chunkNumber++;
                }
            }
            return await uploader.CommitAsync(filename, filename, uid, ct);
        }
    }
}
