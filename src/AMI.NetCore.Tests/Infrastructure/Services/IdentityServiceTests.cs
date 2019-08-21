using System;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Repositories;
using AMI.Core.Services;
using AMI.Domain.Entities;
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
    }
}
