﻿using System.Linq;
using AMI.Core.Configurations;
using AMI.Domain.Enums;
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
            var generalRule0 = configuration.Options.IpRateLimiting?.GeneralRules?[0];
            var ipRule0 = configuration.Options.IpRateLimitPolicies?.IpRules?[0];
            var rule0 = ipRule0?.Rules?[0];

            // Assert
            Assert.AreEqual("CF-Connecting-IP", configuration.Options.ConnectingIpHeaderName);
            Assert.AreEqual(true, configuration.Options.IsDevelopment);

            Assert.IsNotNull(configuration.Options.AuthOptions.JwtOptions);
            Assert.AreEqual("123456", configuration.Options.AuthOptions.JwtOptions.SecretKey);
            Assert.AreEqual("AMI.NetCore.Tests.Issuer", configuration.Options.AuthOptions.JwtOptions.Issuer);
            Assert.AreEqual("AMI.NetCore.Tests.Audience", configuration.Options.AuthOptions.JwtOptions.Audience);

            Assert.IsNotNull(configuration.Options.AuthOptions);
            Assert.IsTrue(configuration.Options.AuthOptions.AllowAnonymous);
            Assert.AreEqual("123456", configuration.Options.AuthOptions?.UserPasswords?.Svc);
            Assert.AreEqual("123456", configuration.Options.AuthOptions?.UserPasswords?.Admin);

            Assert.IsNotNull(configuration.Options.IpRateLimiting);
            Assert.AreEqual("X-Real-IP", configuration.Options.IpRateLimiting.RealIpHeader);
            Assert.AreEqual("ip-pol-pref", configuration.Options.IpRateLimiting.IpPolicyPrefix);
            Assert.AreEqual("127.0.0.1", configuration.Options.IpRateLimiting.IpWhitelist[0]);
            Assert.IsNotNull(configuration.Options.IpRateLimiting.QuotaExceededResponse);
            Assert.AreEqual("test", configuration.Options.IpRateLimiting.QuotaExceededResponse.Content);
            Assert.AreEqual("*:/api/status", configuration.Options.IpRateLimiting.EndpointWhitelist[1]);
            Assert.IsNotNull(generalRule0);
            Assert.AreEqual("*", generalRule0.Endpoint);
            Assert.AreEqual("1s", generalRule0.Period);
            Assert.AreEqual(2, generalRule0.Limit);
            Assert.IsNull(generalRule0.PeriodTimespan);
            Assert.IsNotNull(configuration.Options.IpRateLimiting.ClientWhitelist);
            Assert.AreEqual(0, configuration.Options.IpRateLimiting.ClientWhitelist.Count);
            Assert.IsNotNull(ipRule0);
            Assert.AreEqual("84.247.85.224", ipRule0.Ip);
            Assert.IsNotNull(rule0);
            Assert.AreEqual("*", rule0.Endpoint);
            Assert.AreEqual("1s", rule0.Period);
            Assert.AreEqual(10, rule0.Limit);
            Assert.IsNull(rule0.PeriodTimespan);
        }
    }
}
