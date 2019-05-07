using System;
using System.IO;
using AMI.Core.Helpers;
using AMI.Core.Strategies;
using AMI.Itk.Factories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace AMI.NetFramework.Tests
{
    public class BaseTest
    {
        private readonly ServiceProvider _serviceProvider;

        public BaseTest()
        {
            _serviceProvider = new ServiceCollection()
                .AddSingleton<ILoggerFactory, NullLoggerFactory>()
                .AddSingleton<IItkImageReaderFactory, ItkImageReaderFactory>()
                .AddSingleton<IFileSystemStrategy, FileSystemStrategy>()
                .BuildServiceProvider();
        }

        public T GetService<T>()
        {
            return _serviceProvider.GetService<T>();
        }

        public string GetDataPath(string filename)
        {
            return Path.Combine(FileSystemHelper.BuildCurrentPath(), "data", filename);
        }

        public string GetTempPath()
        {
            string path = Path.Combine(FileSystemHelper.BuildCurrentPath(), "temp", Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(path);
            return path;
        }

        public void DeleteFile(string path)
        {
            File.Delete(path);
        }

        public void DeleteDirectory(string path)
        {
            Directory.Delete(path);
        }
    }
}
