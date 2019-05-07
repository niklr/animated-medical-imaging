using System;
using System.IO;
using AMI.Compress.Readers;
using AMI.Core.Configuration;
using AMI.Core.Extractors;
using AMI.Core.Factories;
using AMI.Core.Helpers;
using AMI.Core.Readers;
using AMI.Core.Serializers;
using AMI.Core.Services;
using AMI.Core.Strategies;
using AMI.Core.Writers;
using AMI.Gif.Writers;
using AMI.Itk.Extractors;
using AMI.Itk.Factories;
using AMI.NetCore.Tests.Mocks.Core.Factories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace AMI.NetCore.Tests
{
    public class BaseTest
    {
        private readonly ServiceProvider _serviceProvider;

        public BaseTest()
        {
            _serviceProvider = new ServiceCollection()
                .AddTransient<ICompressibleReader, SharpCompressReader>()
                .AddTransient<IGifImageWriter, AnimatedGifImageWriter>()
                .AddTransient<IDefaultJsonSerializer, DefaultJsonSerializer>()
                .AddTransient<IDefaultJsonWriter, DefaultJsonWriter>()
                .AddTransient<IImageExtractor, ItkImageExtractor>()
                .AddTransient<IImageService, ImageService>()
                .AddSingleton<ILoggerFactory, NullLoggerFactory>()
                .AddSingleton<IAppInfoFactory, MockAppInfoFactory>()
                .AddSingleton<IItkImageReaderFactory, ItkImageReaderFactory>()
                .AddSingleton<IAmiConfigurationManager, AmiConfigurationManager>()
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
