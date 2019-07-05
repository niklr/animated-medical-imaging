using System.IO;
using System.Linq;
using System.Threading;
using AMI.Core.Configurations;
using AMI.Core.IO.Extractors;
using NUnit.Framework;

namespace AMI.NetCore.Tests.Core.IO.Extractors
{
    [TestFixture]
    public class CompressibleExtractorTests : BaseTest
    {
        [Test]
        public void CompressibleExtractor_ExtractAsync()
        {
            // Arrange
            var extractor = GetService<ICompressibleExtractor>();
            var configuration = GetService<IAppConfiguration>();
            var ct = new CancellationToken();
            var sourcePath = GetDataPath("SMIR.Brain.XX.O.CT.346124.dcm.zip");
            var destinationPath = GetTempPath();

            try
            {
                // Act
                var result = extractor.ExtractAsync(sourcePath, destinationPath, ct).Result;
                var firstEntry = result.FirstOrDefault();
                var lastEntry = result.LastOrDefault();

                // Assert
                Assert.IsTrue(result.Count >= configuration.Options.MaxCompressedEntries);
                Assert.IsNotNull(firstEntry);
                Assert.IsTrue(File.Exists(Path.Combine(destinationPath, firstEntry.Key)));
                Assert.IsNotNull(lastEntry);
                Assert.IsTrue(File.Exists(Path.Combine(destinationPath, lastEntry.Key)));
            }
            finally
            {
                DeleteDirectory(destinationPath);
            }
        }
    }
}
