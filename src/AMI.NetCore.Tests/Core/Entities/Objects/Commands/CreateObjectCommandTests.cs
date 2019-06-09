﻿using System.Threading;
using AMI.Core.Entities.Objects.Commands.Create;
using AMI.Domain.Enums;
using AMI.Domain.Exceptions;
using MediatR;
using NUnit.Framework;

namespace AMI.NetCore.Tests.Core.Entities.Objects.Commands
{
    [TestFixture]
    public class CreateObjectCommandTests : BaseTest
    {
        [Test]
        public void CreateObjectCommand()
        {
            // Arrange
            var mediator = GetService<IMediator>();
            var ct = new CancellationToken();
            var command = new CreateObjectCommand()
            {
                OriginalFilename = "SMIR.Brain_3more.XX.XX.OT.6560.mha",
                SourcePath = GetDataPath("SMIR.Brain_3more.XX.XX.OT.6560.mha")
            };

            // Act
            var result = mediator.Send(command, ct).Result;

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

        [Test]
        public void CreateObjectCommand_Validation_Failures()
        {
            // Arrange
            var mediator = GetService<IMediator>();
            var ct = new CancellationToken();
            var command = new CreateObjectCommand();

            // Act & Assert
            var ex = Assert.ThrowsAsync<ValidationException>(() => mediator.Send(command, ct));
            Assert.AreEqual("One or more validation failures have occurred.", ex.Message);
            Assert.IsNotNull(ex.Failures);
            Assert.AreEqual(2, ex.Failures.Count);
            var firstEntry = ex.Failures[nameof(command.SourcePath)];
            Assert.AreEqual(1, firstEntry.Length);
            Assert.AreEqual("'Source Path' must not be empty.", firstEntry[0]);
            var secondEntry = ex.Failures[nameof(command.OriginalFilename)];
            Assert.AreEqual(1, secondEntry.Length);
            Assert.AreEqual("'Original Filename' must not be empty.", secondEntry[0]);
        }
    }
}
