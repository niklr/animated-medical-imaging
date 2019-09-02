using System.Linq;
using System.Threading;
using AMI.Core.Configurations;
using AMI.Core.IO.Readers;
using NUnit.Framework;

namespace AMI.NetCore.Tests.Core.IO.Readers
{
    [TestFixture]
    public class ArchiveReaderTests : BaseTest
    {
        [TestCase("", false)]
        [TestCase("test", false)]
        [TestCase("test/test", false)]
        [TestCase("test\test", false)]
        [TestCase("test.tar", true)]
        [TestCase("test\test.zip", true)]
        [TestCase("test/test.gz", true)]
        public void ArchiveReader_IsArchive(string path, bool expected)
        {
            // Arrange
            var reader = GetService<IArchiveReader>();

            // Act
            var result = reader.IsArchive(path);

            // Assert
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void ArchiveReader_ReadAsync()
        {
            // Arrange
            var reader = GetService<IArchiveReader>();
            var configuration = GetService<IAppConfiguration>();
            string path = GetDataPath("SMIR.Brain.XX.O.CT.346124.dcm.zip");
            var ct = new CancellationToken();

            // Act
            var result = reader.ReadAsync(path, ct).Result;
            var firstEntry = result.FirstOrDefault();
            var lastEntry = result.LastOrDefault();

            // Assert
            Assert.AreEqual(configuration.Options.MaxArchivedEntries, result.Count);
            Assert.IsNotNull(firstEntry);
            Assert.AreEqual("SMIR.Brain.XX.O.CT.346124-001.dcm", firstEntry.Key);
            Assert.AreEqual(132536, firstEntry.Size);
            Assert.AreEqual(2066, firstEntry.CompressedSize);
            Assert.AreEqual("SMIR.Brain.XX.O.CT.346124-010.dcm", lastEntry.Key);
            Assert.AreEqual(132536, lastEntry.Size);
            Assert.AreEqual(30622, lastEntry.CompressedSize);
        }
    }
}
