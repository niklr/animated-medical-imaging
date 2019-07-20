using System.Threading;
using AMI.Core.Entities.ApiOptions.Queries;
using MediatR;
using NUnit.Framework;

namespace AMI.NetCore.Tests.Core.Entities.ApiOptions.Queries
{
    [TestFixture]
    public class GetQueryTests : BaseTest
    {
        [Test]
        public void GetApiOptionsQuery()
        {
            // Arrange
            var mediator = GetService<IMediator>();
            var ct = new CancellationToken();
            var query = new GetQuery();

            // Act
            var result = mediator.Send(query, ct).Result;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsDevelopment);
            Assert.IsNotNull(result.AuthOptions.JwtOptions);
            Assert.IsTrue(string.IsNullOrWhiteSpace(result.AuthOptions.JwtOptions.SecretKey));
            Assert.IsNotNull(result.AuthOptions.Entities);
            Assert.AreEqual(0, result.AuthOptions.Entities.Count);
        }
    }
}
