using System;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Repositories;
using AMI.Core.Services;
using AMI.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using NUnit.Framework;

namespace AMI.NetCore.Tests.Infrastructure.Services
{
    [TestFixture]
    public class IdentityServiceTests : BaseTest
    {
        [Test]
        public async Task IdentityService_EnsureUsersExistAsync()
        {
            //  Arrange
            var service = GetService<IIdentityService>();
            var context = GetService<IAmiUnitOfWork>();
            var userManager = GetService<UserManager<UserEntity>>();
            var ct = new CancellationToken();
            string username1 = "niklr";
            string username2 = "admin";
            var now = DateTime.UtcNow;

            // Act
            await service.EnsureUsersExistAsync(ct);
            var user1 = await context.UserRepository.GetFirstOrDefaultAsync(e => e.Username == username1, ct);
            var user2 = await context.UserRepository.GetFirstOrDefaultAsync(e => e.Username == username2, ct);

            // Assert
            Assert.AreEqual(2, context.UserRepository.Count());
            Assert.IsNotNull(user1);
            Assert.IsTrue(user1.CreatedDate >= now);
            Assert.IsTrue(user1.ModifiedDate >= now);
            Assert.AreEqual(username1.ToUpper(), user1.NormalizedUsername);
            Assert.IsFalse(string.IsNullOrWhiteSpace(user1.PasswordHash));
            Assert.IsNotNull(user2);
            Assert.IsTrue(user2.CreatedDate >= now);
            Assert.IsTrue(user2.ModifiedDate >= now);
            Assert.AreEqual(username2.ToUpper(), user2.NormalizedUsername);
            Assert.IsFalse(string.IsNullOrWhiteSpace(user2.PasswordHash));
            Assert.IsTrue(userManager.CheckPasswordAsync(user1, "123456").Result);
            Assert.IsTrue(userManager.CheckPasswordAsync(user2, "654321").Result);
            Assert.IsFalse(userManager.CheckPasswordAsync(user1, "invalid").Result);
        }
    }
}
