using AMI.Core.Configurations;
using NUnit.Framework;

namespace AMI.NetCore.Tests.Core.Configurations
{
    [TestFixture]
    public class AppConfigurationTests : BaseTest
    {
        [Test]
        public void AppConfiguration()
        {
            // Arrange
            var configuration = GetService<IAppConfiguration>();

            // Act
            var maxArchivedEntries = configuration.Options.MaxArchivedEntries;
            var maxSizeKilobytes = configuration.Options.MaxSizeKilobytes;
            var timeoutMilliseconds = configuration.Options.TimeoutMilliseconds;
            var workingDirectory = configuration.Options.WorkingDirectory;

            // Assert
            Assert.AreEqual(10, maxArchivedEntries);
            Assert.AreEqual(100000, maxSizeKilobytes);
            Assert.AreEqual(10000, timeoutMilliseconds);
            Assert.AreEqual(@"C:\Temp\AMI.NetCore.Tests", workingDirectory);
        }
    }
}
