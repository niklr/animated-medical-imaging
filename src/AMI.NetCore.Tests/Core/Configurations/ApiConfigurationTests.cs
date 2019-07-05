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
            var generalRule0 = configuration.Options.IpRateLimiting.GeneralRules[0];
            var ipRule0 = configuration.Options.IpRateLimitPolicies.IpRules[0];
            var rule0 = ipRule0.Rules[0];

            // Assert
            Assert.AreEqual("CF-Connecting-IP", configuration.Options.ConnectingIpHeaderName);
            Assert.AreEqual(true, configuration.Options.IsDevelopment);
            Assert.IsNotNull(configuration.Options.IpRateLimiting);
            Assert.AreEqual("X-Real-IP", configuration.Options.IpRateLimiting.RealIpHeader);
            Assert.AreEqual("ip-pol-pref", configuration.Options.IpRateLimiting.IpPolicyPrefix);
            Assert.AreEqual("127.0.0.1", configuration.Options.IpRateLimiting.IpWhitelist[0]);
            Assert.IsNotNull(configuration.Options.IpRateLimiting.QuotaExceededResponse);
            Assert.AreEqual("test", configuration.Options.IpRateLimiting.QuotaExceededResponse.Content);
            Assert.AreEqual("*:/api/status", configuration.Options.IpRateLimiting.EndpointWhitelist[1]);
            Assert.AreEqual("*", generalRule0.Endpoint);
            Assert.AreEqual("1s", generalRule0.Period);
            Assert.AreEqual(2, generalRule0.Limit);
            Assert.IsNull(generalRule0.PeriodTimespan);
            Assert.IsNull(configuration.Options.IpRateLimiting.ClientWhitelist);
            Assert.AreEqual("84.247.85.224", ipRule0.Ip);
            Assert.AreEqual("*", rule0.Endpoint);
            Assert.AreEqual("1s", rule0.Period);
            Assert.AreEqual(10, rule0.Limit);
            Assert.IsNull(rule0.PeriodTimespan);
        }
    }
}
