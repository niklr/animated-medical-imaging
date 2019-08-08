using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Entities.Models;
using AMI.Core.IO.Uploaders;

namespace AMI.NetCore.Tests.Helpers
{
    public static class UploadHelper
    {
        public static async Task<ObjectModel> UploadAsync(
            IChunkedObjectUploader uploader, string dataPath, CancellationToken ct)
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
