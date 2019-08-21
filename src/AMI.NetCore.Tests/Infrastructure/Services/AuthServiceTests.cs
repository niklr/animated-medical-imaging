using AMI.Core.Services;
using AMI.Domain.Enums;
using AMI.NetCore.Tests.Mocks.Core;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace AMI.NetCore.Tests.Infrastructure.Services
{
    [TestFixture]
    public class AuthServiceTests : BaseTest
    {
        [TestCase("00000000-0000-0000-0000-000000000000", false)]
        [TestCase("11111111-1111-1111-1111-111111111111", true)]
        [TestCase("22222222-2222-2222-2222-222222222222", false)]
        [TestCase("33333333-3333-3333-3333-333333333333", false)]
        public void AuthService_IsAuthorized(string ownerId, bool expected)
        {
            //  Arrange
            var service = GetService<IAuthService>();

            // Act
            bool actual = service.IsAuthorized(ownerId);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void AuthService_IsAuthorized_Administrator()
        {
            //  Arrange
            var service = GetService<IAuthService>();
            TestExecutionContext.CurrentContext.CurrentPrincipal = new MockPrincipal(
                "22222222-2222-2222-2222-222222222222", new RoleType[] { RoleType.Administrator });

            // Act
            bool actual = service.IsAuthorized("33333333-3333-3333-3333-333333333333");

            // Assert
            Assert.AreEqual(true, actual);
        }
    }
}
