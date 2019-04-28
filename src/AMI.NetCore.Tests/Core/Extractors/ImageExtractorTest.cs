using System.Threading;
using AMI.Core.Extractors;
using AMI.Core.Strategies;
using AMI.Itk.Extractors;
using AMI.Itk.Readers;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace AMI.NetCore.Tests.Core.Extractors
{
    [TestFixture]
    public class ImageExtractorTest : BaseTest
    {
        [Test]
        public void NetCore_ImageExtractor_Extract()
        {
            var loggerFactory = base.GetService<ILoggerFactory>();
            var fileSystemStrategy = base.GetService<IFileSystemStrategy>();
            var reader = base.GetService<IItkImageReader>();

            string sourcePath = GetDataPath("SMIR.Brain_3more.XX.XX.OT.6560.mha");
            string destinationPath = GetTempPath();
            uint amount = 6;

            IImageExtractor extractor = new ItkImageExtractor(loggerFactory, fileSystemStrategy, reader);
            var ct = new CancellationToken();
            var output = extractor.ExtractAsync(sourcePath, destinationPath, amount, ct).Result;

            // 6 (per axis) * 3 (x, y, z) = 18
            Assert.AreEqual(18, output.Images.Count);
            Assert.AreEqual(5, (int)output.LabelCount);
        }
    }
}
