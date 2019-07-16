using System;
using System.IO;
using System.Threading;
using AMI.Core.Entities.Models;
using AMI.Core.Entities.Objects.Commands.Clear;
using AMI.Core.Entities.Objects.Commands.Create;
using AMI.Core.Repositories;
using MediatR;
using NUnit.Framework;

namespace AMI.NetCore.Tests.Core.Entities.Objects.Commands
{
    [TestFixture]
    public class ClearObjectsCommandTests : BaseTest
    {
        [Test]
        public void ClearObjectsCommand()
        {
            // Arrange
            var mediator = GetService<IMediator>();
            var ct = new CancellationToken();
            string workingDir = GetWorkingDirectoryPath(string.Empty);
            var command = new ClearObjectsCommand();

            // Act
            var directories = Directory.EnumerateDirectories(workingDir);
            var result = mediator.Send(command, ct).Result;

            // Assert
            Assert.IsTrue(result);
            Assert.IsTrue(Directory.Exists(workingDir));
            foreach (var directory in directories)
            {
                Assert.IsFalse(Directory.Exists(directory));
            }
        }

        [Test]
        public void ClearObjectsCommand_RefDate()
        {
            // Arrange
            var context = GetService<IAmiUnitOfWork>();
            var mediator = GetService<IMediator>();
            var ct = new CancellationToken();

            string filename1 = "SMIR.Brain.XX.O.CT.339203.nii";
            string dataPath1 = GetDataPath(filename1);
            var command1 = new CreateObjectCommand()
            {
                OriginalFilename = filename1,
                SourcePath = CreateTempFile(dataPath1)
            };

            string filename2 = "SMIR.Brain.XX.O.CT.346124.dcm";
            string dataPath2 = GetDataPath(filename2);
            var command2 = new CreateObjectCommand()
            {
                OriginalFilename = filename2,
                SourcePath = CreateTempFile(dataPath2)
            };

            string workingDir = GetWorkingDirectoryPath(string.Empty);
            var command = new ClearObjectsCommand()
            {
                RefDate = DateTime.Now
            };

            // Act
            var result1 = mediator.Send(command1, ct).Result;
            UpdateCreatedDate(context, result1, command.RefDate.Value.AddMinutes(-1));
            var result2 = mediator.Send(command2, ct).Result;

            var directories = Directory.EnumerateDirectories(workingDir);
            var result = mediator.Send(command, ct).Result;

            // Assert
            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
            Assert.IsTrue(result);
            Assert.IsTrue(Directory.Exists(workingDir));
            Assert.IsTrue(File.Exists(dataPath1));
            Assert.IsTrue(File.Exists(dataPath2));
            Assert.IsFalse(File.Exists(Path.Combine(workingDir, result1.SourcePath)));
            Assert.IsTrue(File.Exists(Path.Combine(workingDir, result2.SourcePath)));
        }

        private void UpdateCreatedDate(IAmiUnitOfWork context, ObjectModel model, DateTime date)
        {
            var entity = context.ObjectRepository.GetFirstOrDefault(e => e.Id == Guid.Parse(model.Id));
            entity.CreatedDate = date.ToUniversalTime();
            context.ObjectRepository.Update(entity);
            context.SaveChanges();
        }
    }
}
