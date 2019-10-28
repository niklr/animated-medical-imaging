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
        [TestCase(SHARED_GUID_0, false)]
        [TestCase(SHARED_GUID_1, true)]
        [TestCase(SHARED_GUID_2, false)]
        [TestCase(SHARED_GUID_3, false)]
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
                SHARED_GUID_2, new RoleType[] { RoleType.Administrator });

            // Act
            bool actual = service.IsAuthorized(SHARED_GUID_3);

            // Assert
            Assert.AreEqual(true, actual);
        }
    }
}
