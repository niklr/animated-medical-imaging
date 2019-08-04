using System;
using System.Threading;
using AMI.API.Extensions.ServiceProviderServiceExtensions;
using AMI.API.Hubs;
using AMI.API.Observers;
using AMI.Core.Configurations;
using AMI.Core.Entities.Objects.Commands.Clear;
using AMI.Core.Entities.Tasks.Commands.ResetStatus;
using AMI.Core.Services;
using AMI.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using RNS.Framework.Tools;

namespace AMI.API.Extensions.ApplicationBuilderExtensions
{
    /// <summary>
    /// Extensions related to <see cref="IApplicationBuilder"/>.
    /// </summary>
    public static class InitAppExtensions
    {
        /// <summary>
        /// Extension method used to initialize the application.
        /// </summary>
        /// <param name="builder">The application builder.</param>
        /// <param name="gatewayHubContext">The gateway hub context.</param>
        public static void InitApp(this IApplicationBuilder builder, IHubContext<GatewayHub> gatewayHubContext)
        {
            Ensure.ArgumentNotNull(builder, nameof(builder));
            Ensure.ArgumentNotNull(gatewayHubContext, nameof(gatewayHubContext));

            var serviceProvider = builder.ApplicationServices;
            if (serviceProvider == null)
            {
                throw new UnexpectedNullException($"{nameof(IServiceProvider)} could not be retrieved.");
            }

            var loggerFactory = serviceProvider.EnsureGetService<ILoggerFactory>();
            var configuration = serviceProvider.EnsureGetService<IApiConfiguration>();
            var mediator = serviceProvider.EnsureGetService<IMediator>();
            var gatewayObserverService = serviceProvider.EnsureGetService<IGatewayObserverService>();
            var identityService = serviceProvider.EnsureGetService<IIdentityService>();

            var logger = loggerFactory.CreateLogger<Startup>();

            logger.LogInformation("Add gateway observer.");
            gatewayObserverService.Add(new GatewayObserver(gatewayHubContext));

            logger.LogInformation("Ensure users exist.");
            identityService.EnsureUsersExistAsync(default(CancellationToken)).Wait();

            if (configuration.Options.CleanupPeriod > 0)
            {
                logger.LogInformation("Clear expired objects.");
                var command = new ClearObjectsCommand()
                {
                    RefDate = DateTime.UtcNow
                };
                mediator.Send(command, default(CancellationToken)).Wait();
            }

            logger.LogInformation("Reset tasks.");
            mediator.Send(new ResetTaskStatusCommand(), default(CancellationToken)).Wait();

            logger.LogInformation("Application initialization finished.");
        }
    }
}
