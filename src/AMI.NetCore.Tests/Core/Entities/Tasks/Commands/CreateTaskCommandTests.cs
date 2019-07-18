using System;
using System.IO;
using System.Threading;
using AMI.Core.Entities.Objects.Commands.Create;
using AMI.Core.Entities.Results.Commands.ProcessObject;
using AMI.Core.Entities.Results.Commands.ProcessPath;
using AMI.Core.Entities.Tasks.Commands.Create;
using AMI.Domain.Exceptions;
using MediatR;
using NUnit.Framework;

namespace AMI.NetCore.Tests.Core.Entities.Tasks.Commands
{
    [TestFixture]
    public class CreateTaskCommandTests : BaseTest
    {
        [Test]
        public void CreateTaskCommand_Validation_Failures_1()
        {
            // Arrange
            var mediator = GetService<IMediator>();
            var ct = new CancellationToken();
            var command = new CreateTaskCommand();

            // Act & Assert
            var ex = Assert.ThrowsAsync<ValidationException>(() => mediator.Send(command, ct));
            Assert.AreEqual("One or more validation failures have occurred.", ex.Message);
            Assert.IsNotNull(ex.Failures);
            Assert.AreEqual(1, ex.Failures.Count);
            var firstEntry = ex.Failures[nameof(command.Command)];
            Assert.AreEqual(1, firstEntry.Length);
            Assert.AreEqual("'Command' must not be empty.", firstEntry[0]);
        }

        [Test]
        public void CreateTaskCommand_Validation_Failures_2()
        {
            // Arrange
            var mediator = GetService<IMediator>();
            var ct = new CancellationToken();
            var command = new CreateTaskCommand()
            {
                Command = new ProcessObjectCommand()
                {
                }
            };

            // Act & Assert
            var ex = Assert.ThrowsAsync<ValidationException>(() => mediator.Send(command, ct));
            Assert.AreEqual("One or more validation failures have occurred.", ex.Message);
            Assert.IsNotNull(ex.Failures);
            Assert.AreEqual(2, ex.Failures.Count);
            var firstEntry = ex.Failures["Command.AmountPerAxis"];
            Assert.AreEqual(1, firstEntry.Length);
            Assert.AreEqual("'Amount Per Axis' must be greater than '0'.", firstEntry[0]);
        }

        [Test]
        public void CreateTaskCommand_Validation_Failures_3()
        {
            // Arrange
            var mediator = GetService<IMediator>();
            var ct = new CancellationToken();
            var command = new CreateTaskCommand()
            {
                Command = new ProcessPathCommand()
                {
                }
            };

            // Act & Assert
            var ex = Assert.ThrowsAsync<ValidationException>(() => mediator.Send(command, ct));
            Assert.AreEqual("One or more validation failures have occurred.", ex.Message);
            Assert.IsNotNull(ex.Failures);
            Assert.AreEqual(3, ex.Failures.Count);
            var firstEntry = ex.Failures["Command.AmountPerAxis"];
            Assert.AreEqual(1, firstEntry.Length);
            Assert.AreEqual("'Amount Per Axis' must be greater than '0'.", firstEntry[0]);
        }

        [Test]
        public void CreateTaskCommand_Validation_Failures_4()
        {
            // Arrange
            var mediator = GetService<IMediator>();
            var ct = new CancellationToken();
            var command = new CreateTaskCommand()
            {
                Command = new ProcessPathCommand()
                {
                    AmountPerAxis = 1,
                    SourcePath = "test",
                    DestinationPath = "test"
                }
            };

            // Act & Assert
            var ex = Assert.ThrowsAsync<NotSupportedException>(() => mediator.Send(command, ct));
            Assert.AreEqual("The provided command type is not supported.", ex.Message);
        }

        [Test]
        public void CreateTaskCommand()
        {
            // Arrange
            var mediator = GetService<IMediator>();
            var ct = new CancellationToken();
            string filename = "SMIR.Brain.XX.O.CT.346124.dcm";
            string dataPath = GetDataPath(filename);
            var command1 = new CreateObjectCommand()
            {
                OriginalFilename = filename,
                SourcePath = CreateTempFile(dataPath)
            };
            var result1 = mediator.Send(command1, ct).Result;
            var command2 = new CreateTaskCommand()
            {
                Command = new ProcessObjectCommand()
                {
                    Id = result1.Id,
                    AmountPerAxis = 10,
                    OutputSize = 250,
                }
            };

            try
            {
                // Act
                var result2 = mediator.Send(command2, ct).Result;

                // Assert
                Assert.IsNotNull(result2);
            }
            finally
            {
                DeleteObject(result1.Id);
            }
        }
    }
}
