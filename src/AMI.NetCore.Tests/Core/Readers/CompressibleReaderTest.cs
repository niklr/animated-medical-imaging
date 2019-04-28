using System.Threading;
using AMI.Core.Readers;
using NUnit.Framework;

namespace AMI.NetCore.Tests.Core.Readers
{
    [TestFixture]
    public class CompressibleReaderTest : BaseTest
    {
        [Test]
        public void NetCore_CompressibleReader_Read()
        {
            var reader = GetService<ICompressibleReader>();

            string path = GetDataPath("SMIR.Brain.XX.O.CT.346124.zip");
            var ct = new CancellationToken();

            var result = reader.ReadAsync(path, ct).Result;

            Assert.AreEqual(16, result.Count);
        }
    }
}
