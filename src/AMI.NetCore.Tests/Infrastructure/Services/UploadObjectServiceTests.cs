using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Entities.Models;
using AMI.Core.Services;
using NUnit.Framework;

namespace AMI.NetCore.Tests.Infrastructure.Services
{
    [TestFixture]
    public class UploadObjectServiceTests : BaseTest
    {
        private readonly IUploadObjectService service;

        public UploadObjectServiceTests()
        {
            service = GetService<IUploadObjectService>();
        }

        [Test]
        public void UploadObjectService_Upload()
        {
            // Arrange
            var ct = new CancellationToken();
            string filename = "SMIR.Brain_3more.XX.XX.OT.6560.mha";
            var dataPath = GetDataPath(filename);

            // Act
            var result = UploadAsync(dataPath, ct).Result;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Id);
            Assert.AreEqual(filename, result.OriginalFilename);
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
                    await service.UploadAsync(Convert.ToInt32(totalChunks), chunkNumber, uid, stream, ct);

                    long numberOfBytesToReadLeft = stream.Length - stream.Position;
                    if (numberOfBytesToReadLeft < maximumNumberOfBytesToRead)
                    {
                        maximumNumberOfBytesToRead = (int)numberOfBytesToReadLeft;
                        chunk = new byte[maximumNumberOfBytesToRead];
                    }
                    chunkNumber++;
                }
            }
            return await service.CommitAsync(filename, filename, uid, ct);
        }
    }
}
