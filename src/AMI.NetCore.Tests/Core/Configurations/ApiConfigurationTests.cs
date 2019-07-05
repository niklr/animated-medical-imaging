using AMI.Core.Configurations;
using NUnit.Framework;

namespace AMI.NetCore.Tests.Core.Configurations
{
    [TestFixture]
    public class ApiConfigurationTests : BaseTest
    {
        [Test]
        public void ApiConfiguration()
        {
            // Arrange
            var configuration = GetService<IApiConfiguration>();

            // Act
            var connectingIpHeaderName = configuration.ConnectingIpHeaderName;
            var isDevelopment = configuration.IsDevelopment;

            // Assert
            Assert.AreEqual("CF-Connecting-IP", connectingIpHeaderName);
            Assert.AreEqual(true, isDevelopment);
        }
    }
}
