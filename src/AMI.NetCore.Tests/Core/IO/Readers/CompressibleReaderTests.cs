using System.Linq;
using System.Threading;
using AMI.Core.Configurations;
using AMI.Core.IO.Readers;
using NUnit.Framework;

namespace AMI.NetCore.Tests.Core.IO.Readers
{
    [TestFixture]
    public class CompressibleReaderTests : BaseTest
    {
        [Test]
        public void CompressibleReader_ReadAsync()
        {
            // Arrange
            var reader = GetService<ICompressibleReader>();
            var configuration = GetService<IAmiConfigurationManager>();
            string path = GetDataPath("SMIR.Brain.XX.O.CT.346124.dcm.zip");
            var ct = new CancellationToken();

            // Act
            var result = reader.ReadAsync(path, ct).Result;
            var firstEntry = result.FirstOrDefault();
            var lastEntry = result.LastOrDefault();

            // Assert
            Assert.AreEqual(configuration.MaxCompressedEntries, result.Count);
            Assert.IsNotNull(firstEntry);
            Assert.AreEqual("SMIR.Brain.XX.O.CT.346124_Frame_1.dcm", firstEntry.Key);
            Assert.AreEqual(197408, firstEntry.Size);
            Assert.AreEqual(1867, firstEntry.CompressedSize);
            Assert.AreEqual("SMIR.Brain.XX.O.CT.346124_Frame_10.dcm", lastEntry.Key);
            Assert.AreEqual(197408, lastEntry.Size);
            Assert.AreEqual(22517, lastEntry.CompressedSize);
        }
    }
}
