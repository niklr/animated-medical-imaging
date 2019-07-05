using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Configurations;
using AMI.Core.IO.Readers;
using AMI.Core.IO.Writers;
using AMI.Domain.Enums;
using NUnit.Framework;

namespace AMI.NetCore.Tests.Core.IO.Writers
{
    [TestFixture]
    public class CompressibleWriterTests : BaseTest
    {
        [Test]
        public async Task CompressibleWriter_AddFilesAsync()
        {
            // Arrange
            var writer = GetService<ICompressibleWriter>();
            var reader = GetService<ICompressibleReader>();
            var configuration = GetService<IAppConfiguration>();
            var imagesPath = GetImagesPath();
            var tempPath = GetTempPath();
            var zipPath = Path.Combine(tempPath, "test.zip");
            var items = Directory.EnumerateFiles(imagesPath, "*.*", SearchOption.AllDirectories);
            var compression = CompressionType.Deflate;
            var ct = new CancellationToken();

            try
            {
                // Act
                var archive = writer.Create(compression);
                await writer.AddFilesAsync(items, dfp => dfp, en => en.Substring(imagesPath.Length), archive, ct);
                using (Stream stream = File.Create(zipPath))
                {
                    writer.Write(stream, archive);
                }
                var result = await reader.ReadAsync(zipPath, ct);

                // Assert
                Assert.AreEqual(compression, archive.CompressionType);
                Assert.AreEqual(25, items.Count());
                Assert.AreEqual(configuration.Options.MaxCompressedEntries, result.Count);
                Assert.AreEqual(574194, new FileInfo(zipPath).Length);
            }
            finally
            {
                DeleteDirectory(tempPath);
            }
        }

        [Test]
        public async Task CompressibleWriter_CompressionType_None()
        {
            // Arrange
            var writer = GetService<ICompressibleWriter>();
            var reader = GetService<ICompressibleReader>();
            var configuration = GetService<IAppConfiguration>();
            var filename = "SMIR.Brain.XX.O.CT.339203.nii";
            var dataPath = GetDataPath(filename);
            var tempPath = GetTempPath();
            var zipPath = Path.Combine(tempPath, "test.zip");
            var items = new List<string>() { dataPath };
            var compression = CompressionType.None;
            var ct = new CancellationToken();

            try
            {
                // Act
                var archive = writer.Create(compression);
                await writer.AddFilesAsync(items, dfp => dfp, en => filename, archive, ct);
                using (Stream stream = File.Create(zipPath))
                {
                    writer.Write(stream, archive);
                }
                var result = await reader.ReadAsync(zipPath, ct);

                // Assert
                Assert.AreEqual(compression, archive.CompressionType);
                Assert.AreEqual(1, items.Count());
                Assert.AreEqual(1, result.Count);
                Assert.AreEqual(archive.TotalSize, new FileInfo(dataPath).Length);
                Assert.AreEqual(2097660, new FileInfo(zipPath).Length);
            }
            finally
            {
                DeleteDirectory(tempPath);
            }
        }
    }
}
