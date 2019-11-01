using AMI.Core.IO.Builders;
using AMI.Core.IO.Clients;
using AMI.Core.IO.Generators;
using AMI.Core.IO.Readers;
using AMI.Core.IO.Uploaders;
using AMI.Core.IO.Writers;
using AMI.Core.Modules;
using AMI.Core.Repositories;
using AMI.Core.Services;
using AMI.Core.Strategies;
using AMI.Domain.Entities;
using AMI.Infrastructure.IO.Builders;
using AMI.Infrastructure.IO.Clients;
using AMI.Infrastructure.IO.Generators;
using AMI.Infrastructure.IO.Readers;
using AMI.Infrastructure.IO.Uploaders;
using AMI.Infrastructure.IO.Writers;
using AMI.Infrastructure.Modules;
using AMI.Infrastructure.Repositories;
using AMI.Infrastructure.Services;
using AMI.Infrastructure.Stores;
using AMI.Infrastructure.Strategies;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace AMI.Infrastructure.Extensions.ServiceCollectionExtensions
{
    /// <summary>
    /// Extensions related to <see cref="IServiceCollection"/>
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Extension method used to add the default infrastructure services.
        /// </summary>
        /// <param name="services">The service collection.</param>
        public static void AddDefaultInfrastructure(this IServiceCollection services)
        {
            services.AddTransient<ICleanupService, CleanupService>();
            services.AddTransient<ITaskService, TaskService>();
            services.AddTransient<IWebhookService, WebhookService>();
            services.AddScoped<ICommandHandlerModule, CommandHandlerModule>();
            services.AddScoped<IQueryHandlerModule, QueryHandlerModule>();
            services.AddScoped<IAppLogReader, AppLogReader>();
            services.AddScoped<IIdGenerator, IdGenerator>();
            services.AddScoped<IAuditService, AuditService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IEventService, EventService>();
            services.AddScoped<IGatewayService, GatewayService>();
            services.AddScoped<IImageService, ImageService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IDefaultJsonWriter, DefaultJsonWriter>();
            services.AddScoped<IChunkedObjectUploader, ChunkedObjectUploader>();
            services.AddScoped<IUserStore<UserEntity>, UserStore<UserEntity>>();
            services.AddScoped<IRoleStore<RoleEntity>, RoleStore<RoleEntity>>();
            services.AddScoped<IAppLogRepository, AppLogRepository>();
            services.AddSingleton<IJsonHttpClient, JsonHttpClient>();
            services.AddSingleton<IFileSystemStrategy, FileSystemStrategy>();
            services.AddSingleton<IGatewayGroupNameBuilder, GatewayGroupNameBuilder>();
            services.AddSingleton<IGatewayObserverService, GatewayObserverService>();
        }
    }
}
