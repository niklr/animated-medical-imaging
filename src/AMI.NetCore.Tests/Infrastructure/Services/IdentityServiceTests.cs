using System;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Repositories;
using AMI.Core.Services;
using AMI.Domain.Entities;
using AMI.Domain.Enums;
using AMI.NetCore.Tests.Mocks.Core;
using Microsoft.AspNetCore.Identity;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace AMI.NetCore.Tests.Infrastructure.Services
{
    [TestFixture]
    public class IdentityServiceTests : BaseTest
    {
        [TestCase("svc", "123456")]
        [TestCase("admin", "123456")]
        public async Task IdentityService_EnsureUsersExistAsync(string username, string password)
        {
            //  Arrange
            var service = GetService<IIdentityService>();
            var context = GetService<IAmiUnitOfWork>();
            var userManager = GetService<UserManager<UserEntity>>();
            var ct = new CancellationToken();

            // Act
            await service.EnsureUsersExistAsync(ct);
            var now = DateTime.UtcNow;
            var user = await context.UserRepository.GetFirstOrDefaultAsync(e => e.NormalizedUsername == username.ToUpperInvariant(), ct);

            // Assert
            Assert.IsNotNull(user);
            Assert.IsTrue(now >= user.CreatedDate);
            Assert.IsTrue(now >= user.ModifiedDate);
            Assert.AreEqual(username.ToUpperInvariant(), user.NormalizedUsername);
            Assert.IsFalse(string.IsNullOrWhiteSpace(user.PasswordHash));
            Assert.IsTrue(userManager.CheckPasswordAsync(user, password).Result);
            Assert.IsFalse(userManager.CheckPasswordAsync(user, "invalid").Result);
        }

        [TestCase("00000000-0000-0000-0000-000000000000", false)]
        [TestCase("11111111-1111-1111-1111-111111111111", true)]
        [TestCase("22222222-2222-2222-2222-222222222222", false)]
        [TestCase("33333333-3333-3333-3333-333333333333", false)]
        public void IdentityService_IsAuthorized(string ownerId, bool expected)
        {
            //  Arrange
            var service = GetService<IIdentityService>();

            // Act
            bool actual = service.IsAuthorized(ownerId);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void IdentityService_IsAuthorized_Administrator()
        {
            //  Arrange
            var service = GetService<IIdentityService>();
            TestExecutionContext.CurrentContext.CurrentPrincipal = new MockPrincipal(
                "22222222-2222-2222-2222-222222222222", new RoleType[] { RoleType.Administrator });

            // Act
            bool actual = service.IsAuthorized("33333333-3333-3333-3333-333333333333");

            // Assert
            Assert.AreEqual(true, actual);
        }
    }
}
