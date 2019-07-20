using System.Threading;
using AMI.Core.Configurations;
using AMI.Core.Services;
using NUnit.Framework;

namespace AMI.NetCore.Tests.Infrastructure.Services
{
    [TestFixture]
    public class TokenServiceTests : BaseTest
    {
        [Test]
        public void TokenService_DecodeAccessToken()
        {
            // Arrange
            var service = GetService<ITokenService>();
            var configuration = GetService<IApiConfiguration>();
            var ct = new CancellationToken();
            var container = service.CreateAnonymousAsync(ct).Result;

            // Act
            var decoded = service.DecodeAccessToken(container.AccessToken);

            // Assert
            Assert.IsFalse(string.IsNullOrWhiteSpace(decoded.Sub));
            Assert.AreEqual(configuration.Options.AuthOptions.JwtOptions.Issuer, decoded.Iss);
            Assert.AreEqual(configuration.Options.AuthOptions.JwtOptions.Audience, decoded.Aud);
            Assert.IsTrue(decoded.Nbf > 0);
            Assert.IsTrue(decoded.Iat > 0);
            Assert.IsTrue(decoded.IsAnon);
            Assert.IsTrue(decoded.Exp > 0);
        }

        [Test]
        public void TokenService_DecodeIdToken()
        {
            // Arrange
            var service = GetService<ITokenService>();
            var configuration = GetService<IApiConfiguration>();
            var ct = new CancellationToken();
            var container = service.CreateAnonymousAsync(ct).Result;

            // Act
            var decoded = service.DecodeIdToken(container.IdToken);

            // Assert
            Assert.IsFalse(string.IsNullOrWhiteSpace(decoded.Sub));
            Assert.AreEqual(configuration.Options.AuthOptions.JwtOptions.Issuer, decoded.Iss);
            Assert.AreEqual(configuration.Options.AuthOptions.JwtOptions.Audience, decoded.Aud);
            Assert.IsTrue(decoded.Nbf > 0);
            Assert.IsTrue(decoded.Iat > 0);
            Assert.IsTrue(decoded.IsAnon);
            Assert.AreEqual("Anon", decoded.Username);
            Assert.AreEqual("anon@localhost", decoded.Email);
            Assert.IsFalse(decoded.EmailConfirmed);
            Assert.AreEqual(0, decoded.Roles.Count);
        }

        [Test]
        public void TokenService_DecodeRefreshToken()
        {
            // Arrange
            var service = GetService<ITokenService>();
            var configuration = GetService<IApiConfiguration>();
            var ct = new CancellationToken();
            var container = service.CreateAnonymousAsync(ct).Result;

            // Act
            var decoded = service.DecodeRefreshToken(container.RefreshToken);

            // Assert
            Assert.IsFalse(string.IsNullOrWhiteSpace(decoded.Sub));
            Assert.AreEqual(configuration.Options.AuthOptions.JwtOptions.Issuer, decoded.Iss);
            Assert.AreEqual(configuration.Options.AuthOptions.JwtOptions.Audience, decoded.Aud);
            Assert.IsTrue(decoded.Nbf > 0);
            Assert.IsTrue(decoded.Iat > 0);
            Assert.IsTrue(decoded.IsAnon);
        }
    }
}
