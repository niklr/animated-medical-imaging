using AMI.Core.Configurations;
using NUnit.Framework;

namespace AMI.NetCore.Tests.Core.Configurations
{
    [TestFixture]
    public class AmiConfigurationTest : BaseTest
    {
        [Test]
        public void NetCore_AmiConfiguration()
        {
            // Arrange
            var configuration = GetService<IAmiConfigurationManager>();

            // Act
            var maxCompressedEntries = configuration.MaxCompressedEntries;
            var maxSizeKilobytes = configuration.MaxSizeKilobytes;
            var timeoutMilliseconds = configuration.TimeoutMilliseconds;
            var workingDirectory = configuration.WorkingDirectory;

            // Assert
            Assert.AreEqual(10, maxCompressedEntries);
            Assert.AreEqual(100000, maxSizeKilobytes);
            Assert.AreEqual(10000, timeoutMilliseconds);
            Assert.AreEqual(@"C:\Temp\AMI.NetCore.Tests", workingDirectory);
        }
    }
}
