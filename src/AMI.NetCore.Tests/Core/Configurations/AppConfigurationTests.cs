using System.Collections.Generic;
using AMI.Core.Configurations;
using NUnit.Framework;

namespace AMI.NetCore.Tests.Core.Configurations
{
    [TestFixture]
    public class AppConfigurationTests : BaseTest
    {
        [Test]
        public void AppConfiguration_Default()
        {
            // Arrange
            var configuration = GetService<IAppConfiguration>();

            // Assert
            Assert.AreEqual(10, configuration.Options.MaxArchivedEntries);
            Assert.AreEqual(100000, configuration.Options.MaxSizeKilobytes);
            Assert.AreEqual(10000, configuration.Options.TimeoutMilliseconds);
            Assert.AreEqual(@"C:\Temp\AMI.NetCore.Tests", configuration.Options.WorkingDirectory);
        }

        [Test]
        public void AppConfiguration_Override()
        {
            // Arrange
            var dict = new Dictionary<string, string>()
            {
                { "WorkingDirectory", "new_working_directory" }
            };
            OverrideConfigurationSection("AppOptions", dict);
            var configuration = GetService<IAppConfiguration>();

            // Assert
            Assert.AreEqual(10, configuration.Options.MaxArchivedEntries);
            Assert.AreEqual(100000, configuration.Options.MaxSizeKilobytes);
            Assert.AreEqual(10000, configuration.Options.TimeoutMilliseconds);
            Assert.AreEqual("new_working_directory", configuration.Options.WorkingDirectory);
        }
    }
}
