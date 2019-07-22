﻿using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Configurations;
using AMI.Core.Repositories;
using AMI.Core.Services;
using AMI.Domain.Exceptions;
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

        [Test]
        public async Task TokenService_CreateAsync_MaxRefreshTokens()
        {
            // Arrange
            var service = GetService<ITokenService>();
            var identityService = GetService<IIdentityService>();
            var context = GetService<IAmiUnitOfWork>();
            var configuration = GetService<IApiConfiguration>();
            var ct = new CancellationToken();
            await identityService.EnsureUsersExistAsync(ct);
            string username = "niklr";
            var user = await context.UserRepository.GetFirstOrDefaultAsync(e => e.Username == username);

            // Act
            var result1 = await service.CreateAsync(username, ct);
            var token1 = context.TokenRepository.GetFirstOrDefault(e => e.UserId == user.Id);

            // Assert
            Assert.IsNotNull(token1);
            Assert.AreEqual(1, context.TokenRepository.GetQuery(e => e.UserId == user.Id).Count());
            Assert.IsNotNull(result1);
            Assert.IsFalse(string.IsNullOrWhiteSpace(result1.AccessToken));
            Assert.IsFalse(string.IsNullOrWhiteSpace(result1.IdToken));
            Assert.IsFalse(string.IsNullOrWhiteSpace(result1.RefreshToken));

            // Act
            var result2 = await service.CreateAsync(username, ct);
            var token1NotNull = context.TokenRepository.GetFirstOrDefault(e => e.Id == token1.Id);

            // Assert
            // The first refresh token created should still exist
            Assert.IsNotNull(token1NotNull);
            Assert.AreEqual(2, context.TokenRepository.GetQuery(e => e.UserId == user.Id).Count());
            Assert.IsNotNull(result2);
            Assert.IsFalse(string.IsNullOrWhiteSpace(result2.AccessToken));
            Assert.IsFalse(string.IsNullOrWhiteSpace(result2.IdToken));
            Assert.IsFalse(string.IsNullOrWhiteSpace(result2.RefreshToken));

            // Act
            var result3 = await service.CreateAsync(username, ct);
            var token1Null = context.TokenRepository.GetFirstOrDefault(e => e.Id == token1.Id);

            // Assert
            // The first refresh token created should no longer exist
            Assert.IsNull(token1Null);
            // The amount of valid refresh tokens for a single user should not exceed the limit
            Assert.AreEqual(configuration.Options.AuthOptions.MaxRefreshTokens, context.TokenRepository.GetQuery(e => e.UserId == user.Id).Count());
            Assert.IsNotNull(result3);
            Assert.IsFalse(string.IsNullOrWhiteSpace(result3.AccessToken));
            Assert.IsFalse(string.IsNullOrWhiteSpace(result3.IdToken));
            Assert.IsFalse(string.IsNullOrWhiteSpace(result3.RefreshToken));
        }

        [Test]
        public void TokenService_CreateAsync_Invalid_User()
        {
            // Arrange
            var service = GetService<ITokenService>();
            var ct = new CancellationToken();
            string username = "invalid";

            // Act & Assert
            var ex = Assert.ThrowsAsync<UnexpectedNullException>(() => service.CreateAsync(username, ct));
            Assert.AreEqual("Unexpected null exception. User not found.", ex.Message);
        }
    }
}
