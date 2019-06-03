using System.Threading;
using AMI.Core.Entities.Objects.Commands.Process;
using AMI.Domain.Exceptions;
using MediatR;
using NUnit.Framework;

namespace AMI.NetCore.Tests.Core.Entities.Objects.Commands
{
    [TestFixture]
    public class ProcessObjectCommandTest : BaseTest
    {
        [Test]
        public void NetCore_ProcessObjectCommand()
        {
            // Arrange
            var mediator = GetService<IMediator>();
            var ct = new CancellationToken();
            var command = new ProcessObjectCommand()
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
        public void NetCore_ProcessObjectCommand_Validation_Failures()
        {
            // Arrange
            var mediator = GetService<IMediator>();
            var ct = new CancellationToken();
            var command = new ProcessObjectCommand();

            // Act & Assert
            var ex = Assert.ThrowsAsync<ValidationException>(() => mediator.Send(command, ct));
            Assert.AreEqual("One or more validation failures have occurred.", ex.Message);
            Assert.IsNotNull(ex.Failures);
            Assert.AreEqual(2, ex.Failures.Count);
            var firstEntry = ex.Failures[nameof(command.SourcePath)];
            Assert.AreEqual(1, firstEntry.Length);
            Assert.AreEqual("'Source Path' must not be empty.", firstEntry[0]);
            var secondEntry = ex.Failures[nameof(command.DestinationPath)];
            Assert.AreEqual(1, secondEntry.Length);
            Assert.AreEqual("'Destination Path' must not be empty.", secondEntry[0]);
        }
    }
}
