using System.Threading;
using AMI.Core.Readers;
using NUnit.Framework;

namespace AMI.NetCore.Tests.Core.Readers
{
    [TestFixture]
    public class CompressibleReaderTest : BaseTest
    {
        [Test]
        public void NetCore_CompressibleReader_ReadAsync()
        {
            // Arrange
            var reader = GetService<ICompressibleReader>();
            string path = GetDataPath("SMIR.Brain.XX.O.CT.346124.zip");
            var ct = new CancellationToken();

            // Act
            var result = reader.ReadAsync(path, ct).Result;

            // Assert
            Assert.AreEqual(16, result.Count);
        }
    }
}
