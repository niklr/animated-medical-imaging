using System.IO;
using System.Threading;
using AMI.Core.Entities.Objects.Commands.Create;
using AMI.Core.Entities.Objects.Queries.GetObjects;
using AMI.Domain.Exceptions;
using MediatR;
using NUnit.Framework;

namespace AMI.NetCore.Tests.Core.Entities.Objects.Queries
{
    [TestFixture]
    public class GetObjectsQueryTests : BaseTest
    {
        [Test]
        public void GetObjectsQuery()
        {
            // Arrange
            var mediator = GetService<IMediator>();
            var ct = new CancellationToken();
            string filename = "SMIR.Brain_3more.XX.XX.OT.6560.mha";
            string dataPath = GetDataPath(filename);
            var command = new CreateObjectCommand()
            {
                OriginalFilename = filename,
                SourcePath = CreateTempFile(dataPath)
            };
            var commandResult = mediator.Send(command, ct).Result;
            var query = new GetObjectsQuery { Page = 0, Limit = 25 };

            try
            {
                // Act
                var result = mediator.Send(query, ct).Result;

                // Assert
                Assert.IsNotNull(result);
                Assert.IsNotNull(result.Pagination);
                Assert.AreEqual(1, result.Pagination.Total);
                Assert.AreEqual(0, result.Pagination.Page);

                DeleteObject(commandResult.Id);
                Assert.IsFalse(File.Exists(GetWorkingDirectoryPath(commandResult.SourcePath)));
            }
            finally
            {
                DeleteDirectory(Path.GetDirectoryName(command.SourcePath));
            }
        }

        [Test]
        public void GetObjectsQuery_Validation_Failures()
        {
            // Arrange
            var mediator = GetService<IMediator>();
            var ct = new CancellationToken();
            var query = new GetObjectsQuery { Page = -1 };

            // Act & Assert
            var ex = Assert.ThrowsAsync<ValidationException>(() => mediator.Send(query, ct));
            Assert.AreEqual("One or more validation failures have occurred.", ex.Message);
            Assert.IsNotNull(ex.Failures);
            Assert.AreEqual(2, ex.Failures.Count);
            var firstEntry = ex.Failures[nameof(query.Page)];
            Assert.AreEqual(1, firstEntry.Length);
            Assert.AreEqual("'Page' must be greater than or equal to '0'.", firstEntry[0]);
            var secondEntry = ex.Failures[nameof(query.Limit)];
            Assert.AreEqual(3, secondEntry.Length);
            Assert.AreEqual("'Limit' must not be empty.", secondEntry[0]);
            Assert.AreEqual("'Limit' must be greater than '0'.", secondEntry[1]);
            Assert.AreEqual("'Limit' must be one of these values: 10, 25 or 50", secondEntry[2]);
        }
    }
}
