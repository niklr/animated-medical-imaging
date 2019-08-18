using System.Collections.Generic;
using System.IO;
using System.Threading;
using AMI.Core.Configurations;
using AMI.Core.Entities.Models;
using AMI.Core.Entities.Objects.Commands.Create;
using AMI.Core.Providers;
using AMI.Domain.Enums;
using AMI.Domain.Exceptions;
using AMI.NetCore.Tests.Mocks.Core;
using MediatR;
using NUnit.Framework;
using NUnit.Framework.Internal;

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
            var principalProvider = GetService<ICustomPrincipalProvider>();
            var principal = principalProvider.GetPrincipal();
            var ct = new CancellationToken();
            string filename = "SMIR.Brain_3more.XX.XX.OT.6560.mha";
            string dataPath = GetDataPath(filename);
            var command = new CreateObjectCommand()
            {
                OriginalFilename = filename,
                SourcePath = CreateTempFile(dataPath)
            };

            try
            {
                // Act
                var result = mediator.Send(command, ct).Result;
                var fullSourcePath = GetWorkingDirectoryPath(result.SourcePath);

                // Assert
                Assert.IsNotNull(result);
                Assert.IsNotNull(result.Id);
                Assert.IsNotNull(result.CreatedDate);
                Assert.IsNotNull(result.ModifiedDate);
                Assert.AreEqual(DataType.Unknown, result.DataType);
                Assert.AreEqual(FileFormat.Unknown, result.FileFormat);
                Assert.AreEqual(command.OriginalFilename, result.OriginalFilename);
                Assert.IsTrue(File.Exists(dataPath));
                Assert.IsTrue(File.Exists(fullSourcePath));
                Assert.IsFalse(File.Exists(command.SourcePath));
                Assert.AreEqual(new FileInfo(dataPath).Length, new FileInfo(fullSourcePath).Length);
                Assert.AreEqual(principal.Identity.Name, result.UserId);

                DeleteObject(result.Id);
                Assert.IsFalse(File.Exists(fullSourcePath));
            }
            finally
            {
                DeleteDirectory(Path.GetDirectoryName(command.SourcePath));
            }
        }

        [Test]
        public void CreateObjectCommand_Limit_Reached()
        {
            // Arrange
            var mediator = GetService<IMediator>();
            var apiConfiguration = GetService<IApiConfiguration>();
            var ct = new CancellationToken();
            var objectLimit = apiConfiguration.Options.ObjectLimitAnonymous;
            string filename = "SMIR.Brain_3more.XX.XX.OT.6560.mha";
            string dataPath = GetDataPath(filename);
            TestExecutionContext.CurrentContext.CurrentPrincipal = new Mocks.Core.MockPrincipal(
                "22222222-2222-2222-2222-222222222222", new RoleType[] { });

            var results = new List<ObjectModel>();
            for (int i = 0; i < objectLimit; i++)
            {
                var command1 = new CreateObjectCommand()
                {
                    OriginalFilename = filename,
                    SourcePath = CreateTempFile(dataPath)
                };
                var result1 = mediator.Send(command1, ct).Result;

                Assert.IsNotNull(result1);

                results.Add(result1);
                DeleteDirectory(Path.GetDirectoryName(command1.SourcePath));
            }

            try
            {
                // Act
                var command2 = new CreateObjectCommand()
                {
                    OriginalFilename = filename,
                    SourcePath = CreateTempFile(dataPath)
                };
                var ex = Assert.ThrowsAsync<AmiException>(() => mediator.Send(command2, ct));

                // Assert
                Assert.IsNotNull(ex);
                Assert.AreEqual($"The object limit of {objectLimit} has been reached.", ex.Message);
            }
            finally
            {
                foreach (var result in results)
                {
                    DeleteObject(result.Id);
                }
            }
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
