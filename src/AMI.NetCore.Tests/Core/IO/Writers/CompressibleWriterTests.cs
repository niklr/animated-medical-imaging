using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Configurations;
using AMI.Core.IO.Readers;
using AMI.Core.IO.Writers;
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
            var configuration = GetService<IAmiConfigurationManager>();
            var imagesPath = GetImagesPath();
            var tempPath = GetTempPath();
            var zipPath = Path.Combine(tempPath, "test.zip");
            var items = Directory.EnumerateFiles(imagesPath, "*.*", SearchOption.AllDirectories);
            var ct = new CancellationToken();

            try
            {
                // Act
                var archive = writer.Create();
                await writer.AddFilesAsync(items, dfp => dfp, en => en.Substring(imagesPath.Length), archive, ct);
                using (Stream stream = File.Create(zipPath))
                {
                    writer.Write(stream, archive);
                }
                var result = await reader.ReadAsync(zipPath, ct);

                // Assert
                Assert.AreEqual(25, items.Count());
                Assert.AreEqual(configuration.MaxCompressedEntries, result.Count);
            }
            finally
            {
                DeleteDirectory(tempPath);
            }
        }
    }
}
