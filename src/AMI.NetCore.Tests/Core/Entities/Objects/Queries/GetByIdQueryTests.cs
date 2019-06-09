using System.Threading;
using AMI.Core.Entities.Objects.Commands.Create;
using AMI.Core.Entities.Objects.Queries.GetById;
using AMI.Domain.Enums;
using MediatR;
using NUnit.Framework;

namespace AMI.NetCore.Tests.Core.Entities.Objects.Queries
{
    [TestFixture]
    public class GetByIdQueryTests : BaseTest
    {
        [Test]
        public void GetObjectByIdQuery()
        {
            // Arrange
            var mediator = GetService<IMediator>();
            var ct = new CancellationToken();
            var command = new CreateObjectCommand()
            {
                OriginalFilename = "SMIR.Brain_3more.XX.XX.OT.6560.mha",
                SourcePath = GetDataPath("SMIR.Brain_3more.XX.XX.OT.6560.mha")
            };
            var commandResult = mediator.Send(command, ct).Result;
            var query = new GetByIdQuery { Id = commandResult.Id };

            // Act
            var result = mediator.Send(query, ct).Result;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Id);
            Assert.IsNotNull(result.CreatedDate);
            Assert.IsNotNull(result.ModifiedDate);
            Assert.AreEqual(DataType.Unknown, result.DataType);
            Assert.AreEqual(FileFormat.Unknown, result.FileFormat);
            Assert.AreEqual(command.OriginalFilename, result.OriginalFilename);
            Assert.AreEqual(command.SourcePath, result.SourcePath);
        }
    }
}
