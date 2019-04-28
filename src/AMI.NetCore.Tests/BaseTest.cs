﻿using System;
using System.IO;
using AMI.Compress.Readers;
using AMI.Core.Configuration;
using AMI.Core.Helpers;
using AMI.Core.Readers;
using AMI.Core.Strategies;
using AMI.Core.Writers;
using AMI.Gif.Writers;
using AMI.Itk.Readers;
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
                .AddTransient<IItkImageReader, ItkImageReader>()
                .AddTransient<IGifImageWriter, AnimatedGifImageWriter>()
                .AddSingleton<ILoggerFactory, NullLoggerFactory>()
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
            return Path.Combine(FileSystemHelper.BuildCurrentPath("AMI"), "data", filename);
        }

        public string GetTempPath()
        {
            string path = Path.Combine(FileSystemHelper.BuildCurrentPath("AMI"), "temp", Guid.NewGuid().ToString("N"));
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
