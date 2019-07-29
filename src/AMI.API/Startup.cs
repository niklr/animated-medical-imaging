using System.Net;
using System.Reflection;
using System.Threading;
using AMI.API.Extensions.ApplicationBuilderExtensions;
using AMI.API.Extensions.ServiceCollectionExtensions;
using AMI.API.Filters;
using AMI.API.Handlers;
using AMI.API.Hubs;
using AMI.API.Observers;
using AMI.API.Providers;
using AMI.Compress.Extensions.ServiceCollectionExtensions;
using AMI.Core.Behaviors;
using AMI.Core.Configurations;
using AMI.Core.Constants;
using AMI.Core.Entities.AppInfo.Queries;
using AMI.Core.Entities.Models;
using AMI.Core.Entities.Results.Commands.ProcessObject;
using AMI.Core.Factories;
using AMI.Core.IO.Serializers;
using AMI.Core.Mappers;
using AMI.Core.Providers;
using AMI.Core.Queues;
using AMI.Core.Repositories;
using AMI.Core.Services;
using AMI.Domain.Entities;
using AMI.Gif.Extensions.ServiceCollectionExtensions;
using AMI.Infrastructure.Extensions.ServiceCollectionExtensions;
using AMI.Infrastructure.Services;
using AMI.Itk.Extensions.ServiceCollectionExtensions;
using AMI.Persistence.EntityFramework.InMemory;
using AspNetCoreRateLimit;
using FluentValidation.AspNetCore;
using MediatR;
using MediatR.Pipeline;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
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
        /// <param name="env">The hosting environment.</param>
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            HostingEnvironment = env;
            AppInfo = new AppInfoFactory().Create(typeof(Program));
        }

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        private IConfiguration Configuration { get; }

        /// <summary>
        /// Gets the hosting environment.
        /// </summary>
        private IHostingEnvironment HostingEnvironment { get; }

        /// <summary>
        /// Gets the information about the application.
        /// </summary>
        private AppInfo AppInfo { get; }

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
            services.Configure<AppOptions>(Configuration.GetSection("AppOptions"));
            services.Configure<ApiOptions>(Configuration.GetSection("ApiOptions"));
            services.Configure<AspNetCoreRateLimit.IpRateLimitOptions>(Configuration.GetSection("ApiOptions:IpRateLimiting"));
            services.Configure<AspNetCoreRateLimit.IpRateLimitPolicies>(Configuration.GetSection("ApiOptions:IpRateLimitPolicies"));

            // needed to store rate limit counters and ip rules
            services.AddMemoryCache();

            services.AddLogging(builder =>
            {
                builder
                    .AddConfiguration(Configuration.GetSection("Logging"))
                    .AddConsole();
            });

            // Add hosted services
            services.AddHostedService<ProcessTaskHostedService>();
            services.AddHostedService<CleanupHostedService>();

            // Add infrastructure services
            services.AddDefaultInfrastructure();

            // Add compress services
            services.AddDefaultCompress();

            // Add GIF services
            services.AddDefaultGif();

            // Add ITK services
            services.AddDefaultItk();

            // TODO: replace InMemoryUnitOfWork with SQLite
            services.AddScoped<IAmiUnitOfWork, InMemoryUnitOfWork>();
            services.AddSingleton<IApplicationConstants, ApplicationConstants>();
            services.AddSingleton<ICustomPrincipalProvider, CustomPrincipalProvider>();
            services.AddSingleton<IFileExtensionMapper, FileExtensionMapper>();
            services.AddSingleton<IAppInfoFactory, AppInfoFactory>();
            services.AddSingleton<IApiConfiguration, ApiConfiguration>();
            services.AddSingleton<IAppConfiguration, AppConfiguration>();
            services.AddSingleton<ITaskQueue, TaskQueue>();
            services.AddTransient<IDefaultJsonSerializer, DefaultJsonSerializer>();
            services.AddTransient<ICustomExceptionHandler, CustomExceptionHandler>();

            // Add Identity
            services.AddIdentity<UserEntity, RoleEntity>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
            });

            // Add AspNetCoreRateLimit
            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();

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
                    options.Filters.Add(new ProducesResponseTypeAttribute(typeof(ErrorModel), (int)HttpStatusCode.InternalServerError));
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<ProcessCommandValidator>())
                .AddJsonOptions(options =>
                {
                    defaultSerializer.OverrideJsonSerializerSettings(options.SerializerSettings);
                });

            // Add SignalR
            services.AddSignalR()
                .AddJsonProtocol(options =>
                {
                    defaultSerializer.OverrideJsonSerializerSettings(options.PayloadSerializerSettings);
                });

            // AddSignalR must be called before registering custom SignalR services.
            services.AddSingleton<IUserIdProvider, CustomUserIdProvider>();

            // Add AspNetCoreRateLimit configuration (resolvers, counter key builders)
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

            services.AddCustomAuthentication(HostingEnvironment, Configuration);

            // Customise default API behavior
            services.Configure<ApiBehaviorOptions>(options =>
            {
                // Otherwise the RequestValidationBehavior is never triggered
                options.SuppressModelStateInvalidFilter = true;
            });

            // Customise the Swagger specification
            services.AddCustomOpenApiDocument(AppInfo);
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">The application builder.</param>
        /// <param name="env">The hosting environment.</param>
        /// <param name="gatewayHubContext">The gateway hub context.</param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IHubContext<GatewayHub> gatewayHubContext)
        {
            var serviceProvider = app.ApplicationServices;

            if (env.IsDevelopment())
            {
                // app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseThrottleMiddleware();

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
                options.SwaggerRoutes.Add(new SwaggerUi3Route($"v{AppInfo.AppVersion}", "/specification.json"));
            });

            app.UseAuthentication();

            app.UseMvc();

            app.UseSignalR(routes =>
            {
                routes.MapHub<GatewayHub>("/gateway");
            });

            var identityService = serviceProvider.GetService<IIdentityService>();
            identityService.EnsureUsersExistAsync(default(CancellationToken)).Wait();

            var gatewayObserverService = serviceProvider.GetService<IGatewayObserverService>();
            gatewayObserverService.Add(new GatewayObserver(gatewayHubContext));
        }
    }
}
