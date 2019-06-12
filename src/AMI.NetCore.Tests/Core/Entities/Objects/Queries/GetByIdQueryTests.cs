using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Entities.Objects.Commands.Create;
using AMI.Core.Entities.Objects.Queries.GetById;
using AMI.Domain.Enums;
using AMI.Domain.Exceptions;
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
            string filename = "SMIR.Brain_3more.XX.XX.OT.6560.mha";
            string dataPath = GetDataPath(filename);
            var command = new CreateObjectCommand()
            {
                OriginalFilename = filename,
                SourcePath = CreateTempFile(dataPath)
            };
            var commandResult = mediator.Send(command, ct).Result;
            var query = new GetByIdQuery { Id = commandResult.Id };

            try
            {
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
                Assert.IsTrue(File.Exists(dataPath));
                Assert.IsTrue(File.Exists(result.SourcePath));
                Assert.IsFalse(File.Exists(command.SourcePath));
            }
            finally
            {
                DeleteDirectory(Path.GetDirectoryName(command.SourcePath));
                // TODO: delete object
            }
        }

        [Test]
        public void GetObjectByIdQuery_NotFoundException()
        {
            // Arrange
            var mediator = GetService<IMediator>();
            var ct = new CancellationToken();
            var query = new GetByIdQuery { Id = Guid.NewGuid().ToString() };

            // Act
            async Task func() => await mediator.Send(query, ct);

            // Assert
            Assert.ThrowsAsync<NotFoundException>(func);
        }

        [Test]
        public void GetObjectByIdQuery_Validation_Failures()
        {
            // Arrange
            var mediator = GetService<IMediator>();
            var ct = new CancellationToken();
            var query = new GetByIdQuery { Id = "-1" };

            // Act & Assert
            var ex = Assert.ThrowsAsync<ValidationException>(() => mediator.Send(query, ct));
            Assert.AreEqual("One or more validation failures have occurred.", ex.Message);
            Assert.IsNotNull(ex.Failures);
            Assert.AreEqual(1, ex.Failures.Count);
            var firstEntry = ex.Failures[nameof(query.Id)];
            Assert.AreEqual(1, firstEntry.Length);
            Assert.AreEqual("The specified condition was not met for 'Id'.", firstEntry[0]);
        }
    }
}
