using System.Threading;
using AMI.Core.Entities.Results.Commands.ProcessPath;
using AMI.Domain.Exceptions;
using MediatR;
using NUnit.Framework;

namespace AMI.NetCore.Tests.Core.Entities.Results.Commands
{
    [TestFixture]
    public class ProcessPathCommandTests : BaseTest
    {
        [Test]
        public void ProcessPathCommand()
        {
            // Arrange
            var mediator = GetService<IMediator>();
            var ct = new CancellationToken();
            var command = new ProcessPathCommand()
            {
                SourcePath = GetDataPath("SMIR.Brain_3more.XX.XX.OT.6560.mha"),
                DestinationPath = GetTempPath(),
                AmountPerAxis = 6
            };

            try
            {
                // Act
                var result = mediator.Send(command, ct).Result;

                // Assert
                // 6 (per axis) * 3 (x, y, z) = 18
                Assert.AreEqual(18, result.Images.Count);
                Assert.AreEqual(5, result.LabelCount);
            }
            finally
            {
                DeleteDirectory(command.DestinationPath);
            }
        }

        [Test]
        public void ProcessPathCommand_Validation_Failures()
        {
            // Arrange
            var mediator = GetService<IMediator>();
            var ct = new CancellationToken();
            var command = new ProcessPathCommand();

            // Act & Assert
            var ex = Assert.ThrowsAsync<ValidationException>(() => mediator.Send(command, ct));
            Assert.AreEqual("One or more validation failures have occurred.", ex.Message);
            Assert.IsNotNull(ex.Failures);
            Assert.AreEqual(3, ex.Failures.Count);
            var firstEntry = ex.Failures[nameof(command.AmountPerAxis)];
            Assert.AreEqual(1, firstEntry.Length);
            Assert.AreEqual("'Amount Per Axis' must be greater than '0'.", firstEntry[0]);
            var secondEntry = ex.Failures[nameof(command.SourcePath)];
            Assert.AreEqual(1, secondEntry.Length);
            Assert.AreEqual("'Source Path' must not be empty.", secondEntry[0]);
            var thirdEntry = ex.Failures[nameof(command.DestinationPath)];
            Assert.AreEqual(1, thirdEntry.Length);
            Assert.AreEqual("'Destination Path' must not be empty.", thirdEntry[0]);
        }
    }
}
