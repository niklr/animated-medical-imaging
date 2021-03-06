﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using AMI.Compress.Extensions.ServiceCollectionExtensions;
using AMI.Core.Behaviors;
using AMI.Core.Configurations;
using AMI.Core.Constants;
using AMI.Core.Entities.Models;
using AMI.Core.Entities.Objects.Commands.Delete;
using AMI.Core.Entities.Results.Commands.ProcessObject;
using AMI.Core.Factories;
using AMI.Core.Helpers;
using AMI.Core.IO.Serializers;
using AMI.Core.Mappers;
using AMI.Core.Providers;
using AMI.Core.Repositories;
using AMI.Core.Services;
using AMI.Domain.Entities;
using AMI.Gif.Extensions.ServiceCollectionExtensions;
using AMI.Hangfire.Extensions;
using AMI.Infrastructure.Extensions.ServiceCollectionExtensions;
using AMI.Infrastructure.Services;
using AMI.Itk.Extensions.ServiceCollectionExtensions;
using AMI.NetCore.Tests.Mocks.Core.Factories;
using AMI.NetCore.Tests.Mocks.Core.Providers;
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
        public const string SHARED_GUID_0 = "00000000-0000-0000-0000-000000000000";
        public const string SHARED_GUID_1 = "11111111-1111-1111-1111-111111111111";
        public const string SHARED_GUID_2 = "22222222-2222-2222-2222-222222222222";
        public const string SHARED_GUID_3 = "33333333-3333-3333-3333-333333333333";

        private readonly ServiceCollection services;

        public BaseTest()
        {
            var configuration = CreateConfigurationBuilder().Build();

            services = new ServiceCollection();

            services.AddOptions();
            services.Configure<AppOptions>(configuration.GetSection("AppOptions"));
            services.Configure<ApiOptions>(configuration.GetSection("ApiOptions"));
            services.AddLogging();

            // Add infrastructure services
            services.AddDefaultInfrastructure();

            // Add compress services
            services.AddDefaultCompress();

            // Add GIF services
            services.AddDefaultGif();

            // Add ITK services
            services.AddDefaultItk();

            // Add Hangfire
            services.AddTestHangfire();

            services.AddScoped<IAmiUnitOfWork, InMemoryUnitOfWork>();
            services.AddScoped<IIdentityService, IdentityService>();
            services.AddSingleton<IApplicationConstants, ApplicationConstants>();
            services.AddSingleton<ICustomPrincipalProvider, MockPrincipalProvider>();
            services.AddSingleton<ILoggerFactory, NullLoggerFactory>();
            services.AddSingleton<IAppInfoFactory, MockAppInfoFactory>();
            services.AddSingleton<IApiConfiguration, ApiConfiguration>();
            services.AddSingleton<IAppConfiguration, AppConfiguration>();
            services.AddSingleton<IFileExtensionMapper, FileExtensionMapper>();
            services.AddTransient<IDefaultJsonSerializer, DefaultJsonSerializer>();

            services.AddIdentity<UserEntity, RoleEntity>(options => {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
            });

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

            ServiceProvider = services.BuildServiceProvider();
        }

        public IServiceProvider ServiceProvider { get; private set; }

        public T GetService<T>()
        {
            return ServiceProvider.GetService<T>();
        }

        public string GetDataPath(string filename)
        {
            return Path.Combine(FileSystemHelper.BuildCurrentPath(), "data", filename);
        }

        public string GetTestPath()
        {
            return Path.Combine(FileSystemHelper.BuildCurrentPath(), "data", "test");
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

        public void OverrideAppOptions(IDictionary<string, string> dict)
        {
            var configurationRoot = CreateConfigurationBuilder().Build();
            var section = configurationRoot.GetSection("AppOptions");
            foreach (var kvp in dict)
            {
                section[kvp.Key] = kvp.Value;
            }
            services.Configure<AppOptions>(section);
            ServiceProvider = services.BuildServiceProvider();
        }

        private IConfigurationBuilder CreateConfigurationBuilder()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        }
    }
}
