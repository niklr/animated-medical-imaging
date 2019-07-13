using AMI.Core.Configurations;
using AMI.Core.Extensions.FileSystemExtensions;
using AMI.Core.Helpers;
using AMI.Core.Strategies;
using NUnit.Framework;

namespace AMI.NetCore.Tests.Extensions
{
    [TestFixture]
    public class FileSystemExtensionsTests : BaseTest
    {
        [Test]
        public void FileSystemExtensions_BuildAbsolutePath()
        {
            // Arrange
            var strategy = GetService<IFileSystemStrategy>();
            var configuration = GetService<IAppConfiguration>();
            var fs = strategy.Create(configuration.Options.WorkingDirectory);
            var expected = FileSystemHelper.BuildCurrentPath(string.Empty).TrimEnd(new[] { '/', '\\' });

            // Act
            var result1 = fs.BuildAbsolutePath(".");
            var result2 = fs.BuildAbsolutePath("test");

            // Assert
            Assert.AreEqual(expected, result1);
            Assert.AreEqual(fs.Path.Combine(expected, "test"), result2);
        }

        [TestCase("temp")]
        [TestCase("temp\test")]
        [TestCase("temp\test.txt")]
        public void FileSystemExtensions_BuildAbsolutePath_1(string path)
        {
            // Arrange
            var strategy = GetService<IFileSystemStrategy>();
            var configuration = GetService<IAppConfiguration>();
            var fs = strategy.Create(configuration.Options.WorkingDirectory);
            var expected = fs.Path.Combine(FileSystemHelper.BuildCurrentPath(string.Empty), path);

            // Act
            var result = fs.BuildAbsolutePath(path);

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestCase("test", false)]
        [TestCase(@"C:\temp", true)]
        public void FileSystemExtensions_IsDirectory(string path, bool expected)
        {
            // Arrange
            var strategy = GetService<IFileSystemStrategy>();
            var fs = strategy.Create(path);

            // Act
            var result = fs.IsDirectory(path);

            // Assert
            Assert.AreEqual(expected, result);
        }
    }
}
