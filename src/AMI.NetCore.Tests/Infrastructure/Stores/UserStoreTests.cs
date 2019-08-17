using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Services;
using AMI.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using NUnit.Framework;

namespace AMI.NetCore.Tests.Infrastructure.Stores
{
    [TestFixture]
    public class UserStoreTests : BaseTest
    {
        [TestCase("svc")]
        [TestCase("admin")]
        public async Task UserStore_FindByNameAsync(string username)
        {
            // Arrange
            var service = GetService<IIdentityService>();
            var store = GetService<IUserStore<UserEntity>>();
            var ct = new CancellationToken();
            await service.EnsureUsersExistAsync(ct);

            // Act
            var user = await store.FindByNameAsync(username.ToUpperInvariant(), ct);

            // Assert
            Assert.IsNotNull(user);
            Assert.AreEqual(username.ToUpperInvariant(), user.NormalizedUsername);
        }

        [TestCase("svc", new string[] { "User" })]
        [TestCase("admin", new string[] { "User", "Administrator" })]
        public async Task UserStore_GetRolesAsync(string username, string[] expectedRoles)
        {
            // Arrange
            var service = GetService<IIdentityService>();
            var userManager = GetService<UserManager<UserEntity>>();
            var ct = new CancellationToken();
            await service.EnsureUsersExistAsync(ct);
            var user = await userManager.FindByNameAsync(username);

            // Act
            var roles = await userManager.GetRolesAsync(user);

            // Assert
            Assert.IsNotNull(roles);
            Assert.AreEqual(expectedRoles.Length, roles.Count);
            foreach (var role in expectedRoles)
            {
                Assert.IsTrue(roles.Contains(role));
            }
        }

        [TestCase("Administrator", new string[] { "ADMIN" })]
        [TestCase("Test", new string[] { })]
        public async Task UserStore_GetUsersInRoleAsync(string role, string[] expectedUsers)
        {
            // Arrange
            var service = GetService<IIdentityService>();
            var userManager = GetService<UserManager<UserEntity>>();
            var ct = new CancellationToken();
            await service.EnsureUsersExistAsync(ct);

            // Act
            var users = await userManager.GetUsersInRoleAsync(role);

            // Assert
            Assert.IsNotNull(users);
            Assert.AreEqual(expectedUsers.Length, users.Count);
            foreach (var user in users)
            {
                Assert.IsTrue(expectedUsers.Contains(user.Username.ToUpperInvariant()));
            }
        }

        [TestCase("admin", "Administrator", true)]
        [TestCase("admin", "Test", false)]
        public async Task UserStore_IsInRoleAsync(string username, string role, bool expected)
        {
            // Arrange
            var service = GetService<IIdentityService>();
            var userManager = GetService<UserManager<UserEntity>>();
            var ct = new CancellationToken();
            await service.EnsureUsersExistAsync(ct);
            var user = await userManager.FindByNameAsync(username.ToUpperInvariant());

            // Act
            var result = await userManager.IsInRoleAsync(user, role);

            // Assert
            Assert.AreEqual(expected, result);
        }
    }
}
