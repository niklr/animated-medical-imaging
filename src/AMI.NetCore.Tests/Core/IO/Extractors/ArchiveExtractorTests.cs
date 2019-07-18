using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Configurations;
using AMI.Core.IO.Extractors;
using NUnit.Framework;

namespace AMI.NetCore.Tests.Core.IO.Extractors
{
    [TestFixture]
    public class ArchiveExtractorTests : BaseTest
    {
        [TestCase("SMIR.Brain.XX.O.CT.346124.dcm.zip", 10)]
        [TestCase("SMIR.Brain.XX.O.CT.346124.dcm.tar", 10)]
        [TestCase("SMIR.Brain.XX.O.CT.346124.dcm.tar.gz", 10)]
        public void ArchiveExtractor_ExtractAsync(string filename, int expectedCount)
        {
            // Arrange
            var extractor = GetService<IArchiveExtractor>();
            var configuration = GetService<IAppConfiguration>();
            var ct = new CancellationToken();
            var sourcePath = GetDataPath(filename);
            var destinationPath = GetTempPath();

            try
            {
                // Act
                var result = extractor.ExtractAsync(sourcePath, destinationPath, ct).Result;
                var firstEntry = result.FirstOrDefault();
                var lastEntry = result.LastOrDefault();

                // Assert
                Assert.IsTrue(result.Count >= configuration.Options.MaxArchivedEntries);
                Assert.IsNotNull(firstEntry);
                Assert.IsTrue(File.Exists(Path.Combine(destinationPath, firstEntry.Key)));
                Assert.IsNotNull(lastEntry);
                Assert.IsTrue(File.Exists(Path.Combine(destinationPath, lastEntry.Key)));
                Assert.AreEqual(expectedCount, result.Count);
                Assert.AreEqual(expectedCount, Directory.GetFiles(destinationPath).Count());
            }
            finally
            {
                DeleteDirectory(destinationPath);
            }
        }

        [TestCase]
        public void ArchiveExtractor_ExtractAsync_NotSupportedException()
        {
            // Arrange
            var extractor = GetService<IArchiveExtractor>();
            var configuration = GetService<IAppConfiguration>();
            var ct = new CancellationToken();
            var sourcePath = GetDataPath("SMIR.Brain_3more.XX.XX.OT.6560.mha.tar.tar.gz");
            var destinationPath = GetTempPath();

            try
            {
                // Act
                async Task func() => await extractor.ExtractAsync(sourcePath, destinationPath, ct);

                // Assert
                var ex = Assert.ThrowsAsync<NotSupportedException>(func);
                Assert.AreEqual("The archive contains too many levels.", ex.Message);
            }
            finally
            {
                DeleteDirectory(destinationPath);
            }
        }
    }
}
