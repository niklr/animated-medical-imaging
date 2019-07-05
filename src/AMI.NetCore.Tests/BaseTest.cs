using System;
using System.IO;
using System.Reflection;
using AMI.Compress.Extractors;
using AMI.Compress.Readers;
using AMI.Compress.Writers;
using AMI.Core.Behaviors;
using AMI.Core.Configurations;
using AMI.Core.Constants;
using AMI.Core.Entities.Models;
using AMI.Core.Entities.Objects.Commands.Delete;
using AMI.Core.Entities.Results.Commands.ProcessObject;
using AMI.Core.Factories;
using AMI.Core.Helpers;
using AMI.Core.IO.Extractors;
using AMI.Core.IO.Readers;
using AMI.Core.IO.Serializers;
using AMI.Core.IO.Uploaders;
using AMI.Core.IO.Writers;
using AMI.Core.Queues;
using AMI.Core.Repositories;
using AMI.Core.Services;
using AMI.Core.Strategies;
using AMI.Core.Workers;
using AMI.Gif.Writers;
using AMI.Infrastructure.IO.Uploaders;
using AMI.Infrastructure.IO.Writers;
using AMI.Infrastructure.Services;
using AMI.Infrastructure.Strategies;
using AMI.Itk.Extractors;
using AMI.Itk.Factories;
using AMI.NetCore.Tests.Mocks.Core.Factories;
using AMI.Persistence.EntityFramework.InMemory;
using FluentValidation;
using MediatR;
using MediatR.Pipeline;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace AMI.NetCore.Tests
{
    public class BaseTest
    {
        private readonly ServiceProvider serviceProvider;

        public BaseTest()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var services = new ServiceCollection();

            services.AddOptions();
            services.Configure<AppOptions>(configuration.GetSection("AppOptions"));
            services.Configure<ApiOptions>(configuration.GetSection("ApiOptions"));
            services.AddLogging();
            services.AddScoped<IAmiUnitOfWork, InMemoryUnitOfWork>();
            services.AddScoped<IIdGenService, IdGenService>();
            services.AddScoped<IImageService, ImageService>();
            services.AddScoped<IChunkedObjectUploader, ChunkedObjectUploader>();
            services.AddSingleton<IApplicationConstants, ApplicationConstants>();
            services.AddSingleton<ILoggerFactory, NullLoggerFactory>();
            services.AddSingleton<IAppInfoFactory, MockAppInfoFactory>();
            services.AddSingleton<IItkImageReaderFactory, ItkImageReaderFactory>();
            services.AddSingleton<IApiConfiguration, ApiConfiguration>();
            services.AddSingleton<IAppConfiguration, AppConfiguration>();
            services.AddSingleton<IFileSystemStrategy, FileSystemStrategy>();
            services.AddSingleton<ITaskQueue, TaskQueue>();
            services.AddSingleton<ITaskWorker, TaskWorker>();
            services.AddTransient<ICompressibleReader, SharpCompressReader>();
            services.AddTransient<ICompressibleWriter, SharpCompressWriter>();
            services.AddTransient<IGifImageWriter, AnimatedGifImageWriter>();
            services.AddTransient<IDefaultJsonSerializer, DefaultJsonSerializer>();
            services.AddTransient<IDefaultJsonWriter, DefaultJsonWriter>();
            services.AddTransient<IImageExtractor, ItkImageExtractor>();
            services.AddTransient<ICompressibleExtractor, SharpCompressExtractor>();

            // Add MediatR
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPreProcessorBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPerformanceBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));
            services.AddMediatR(typeof(ProcessCommandHandler).GetTypeInfo().Assembly);

            // Add FluentValidation
            AssemblyScanner.FindValidatorsInAssemblyContaining<ProcessCommandValidator>().ForEach(pair =>
            {
                // filter out validators that are not needed here
                services.AddTransient(pair.InterfaceType, pair.ValidatorType);
            });

            // Add DbContext
            services.AddDbContext<InMemoryDbContext>(options =>
            {
                options.UseInMemoryDatabase(Guid.NewGuid().ToString());
                options.ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            });

            serviceProvider = services.BuildServiceProvider();
        }

        public T GetService<T>()
        {
            return serviceProvider.GetService<T>();
        }

        public string GetDataPath(string filename)
        {
            return Path.Combine(FileSystemHelper.BuildCurrentPath(), "data", filename);
        }

        public string GetWorkingDirectoryPath(string path)
        {
            var configuration = GetService<IAppConfiguration>();
            return Path.Combine(configuration.Options.WorkingDirectory, path);
        }

        public string GetImagesPath()
        {
            return Path.Combine(FileSystemHelper.BuildCurrentPath(), "assets", "images");
        }

        public string GetTempPath()
        {
            string path = Path.Combine(FileSystemHelper.BuildCurrentPath(), "temp", Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(path);
            return path;
        }

        public string CreateTempFile(string sourcePath)
        {
            var constants = GetService<IApplicationConstants>();
            string filename = string.Concat(Guid.NewGuid().ToString("N"), constants.DefaultFileExtension);
            string path = Path.Combine(GetTempPath(), filename);
            File.Copy(sourcePath, path);
            return path;
        }

        public void DeleteFile(string path)
        {
            File.Delete(path);
        }

        public void DeleteDirectory(string path)
        {
            Directory.Delete(path, true);
        }

        public void DeleteObject(string id)
        {
            var mediator = GetService<IMediator>();
            var command = new DeleteObjectCommand()
            {
                Id = id
            };
            mediator.Send(command);
        }
    }
}
