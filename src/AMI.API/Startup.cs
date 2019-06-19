﻿using System;
using System.Net;
using System.Reflection;
using AMI.API.Configuration;
using AMI.API.Extensions.ApplicationBuilderExtensions;
using AMI.API.Filters;
using AMI.API.Handlers;
using AMI.Core.Behaviors;
using AMI.Core.Configurations;
using AMI.Core.Constants;
using AMI.Core.Entities.ApplicationInformation.Queries;
using AMI.Core.Entities.Models;
using AMI.Core.Entities.Results.Commands.ProcessObject;
using AMI.Core.Factories;
using AMI.Core.IO.Serializers;
using AMI.Core.IO.Uploaders;
using AMI.Core.Queues;
using AMI.Core.Repositories;
using AMI.Core.Services;
using AMI.Core.Strategies;
using AMI.Core.Workers;
using AMI.Infrastructure.IO.Uploaders;
using AMI.Infrastructure.Services;
using AMI.Infrastructure.Strategies;
using AMI.Persistence.EntityFramework.InMemory;
using FluentValidation.AspNetCore;
using MediatR;
using MediatR.Pipeline;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSwag.AspNetCore;

namespace AMI.API
{
    /// <summary>
    /// The startup of the application.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Acts as a root for all in-memory databases such that they will be available across context instances and service providers.
        /// </summary>
        public static readonly InMemoryDatabaseRoot InMemoryDatabaseRoot = new InMemoryDatabaseRoot();

        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services">The service collection.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            var allowedCorsOrigins = Configuration["AllowedCorsOrigins"]?.Split(',') ?? new string[0];

            services.AddCors(options =>
            {
                options.AddPolicy(
                    "AllowSpecificOrigins",
                    builder => builder.WithOrigins(allowedCorsOrigins).AllowCredentials().AllowAnyMethod().AllowAnyHeader());
            });

            var defaultSerializer = new DefaultJsonSerializer();

            services.AddOptions();
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            services.AddLogging(builder =>
            {
                builder
                    .AddConfiguration(Configuration.GetSection("Logging"))
                    .AddConsole();
            });

            // Add hosted services
            services.AddHostedService<ProcessObjectHostedService>();

            // TODO: replace InMemoryUnitOfWork with SQLite
            services.AddScoped<IAmiUnitOfWork, InMemoryUnitOfWork>();
            services.AddScoped<IIdGenService, IdGenService>();
            services.AddScoped<IChunkedObjectUploader, ChunkedObjectUploader>();
            services.AddSingleton<IApplicationConstants, ApplicationConstants>();
            services.AddSingleton<IFileSystemStrategy, FileSystemStrategy>();
            services.AddSingleton<IAppInfoFactory, AppInfoFactory>();
            services.AddSingleton<IApiConfiguration, ApiConfiguration>();
            services.AddSingleton<IAmiConfigurationManager, AmiConfigurationManager>();
            services.AddSingleton<ITaskQueue, TaskQueue>();
            services.AddSingleton<ITaskWorker, TaskWorker>();
            services.AddTransient<IDefaultJsonSerializer, DefaultJsonSerializer>();
            services.AddTransient<ICustomExceptionHandler, CustomExceptionHandler>();

            // Add MediatR
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPreProcessorBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPerformanceBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));
            services.AddMediatR(typeof(GetQueryHandler).GetTypeInfo().Assembly);

            // Add DbContext
            services.AddDbContext<InMemoryDbContext>(options =>
            {
                options.UseInMemoryDatabase("AmiInMemoryDb", InMemoryDatabaseRoot);
                options.ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning)); // remove when not using InMemory context
            });

            services.AddMvc(options =>
                {
                    options.Filters.Add(typeof(CustomExceptionFilterAttribute));
                    options.Filters.Add(new ProducesResponseTypeAttribute(typeof(ErrorModel), (int)HttpStatusCode.BadRequest));
                    options.Filters.Add(new ProducesResponseTypeAttribute(typeof(ErrorModel), (int)HttpStatusCode.Unauthorized));
                    options.Filters.Add(new ProducesResponseTypeAttribute(typeof(ErrorModel), (int)HttpStatusCode.Forbidden));
                    options.Filters.Add(new ProducesResponseTypeAttribute(typeof(ErrorModel), (int)HttpStatusCode.NotFound));
                    options.Filters.Add(new ProducesResponseTypeAttribute(typeof(ErrorModel), (int)HttpStatusCode.Conflict));
                    options.Filters.Add(new ProducesResponseTypeAttribute(typeof(ErrorModel), (int)HttpStatusCode.InternalServerError));
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<ProcessCommandValidator>())
                .AddJsonOptions(options =>
                {
                    defaultSerializer.OverrideJsonSerializerSettings(options.SerializerSettings);
                });

            // Customise default API behavior
            services.Configure<ApiBehaviorOptions>(options =>
            {
                // Otherwise the RequestValidationBehavior is never triggered
                options.SuppressModelStateInvalidFilter = true;
            });
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">The application builder.</param>
        /// <param name="env">The hosting environment.</param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                // app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseCors("AllowSpecificOrigins");

            app.UseCustomExceptionMiddleware();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            // Add OpenAPI/Swagger middlewares
            // https://github.com/RSuter/NSwag/wiki/Assembly-loading#net-core
            app.UseReDoc(options =>
            {
                options.Path = "/redoc";
                options.DocumentPath = "/specification.json";
            });

            // Serves the Swagger UI 3 web ui to view the OpenAPI/Swagger documents by default on `/swagger`
            app.UseSwaggerUi3(options =>
            {
                options.Path = "/swagger";
                options.SwaggerRoutes.Add(new SwaggerUi3Route("v0.0.1", "/specification.json"));
            });

            app.UseMvc();
        }
    }
}
