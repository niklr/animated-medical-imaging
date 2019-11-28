using System;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Configurations;
using AMI.Core.Entities.Objects.Commands.Clear;
using AMI.Core.Services;
using AMI.Core.Wrappers;
using MediatR;
using Microsoft.Extensions.Logging;
using RNS.Framework.Tools;

namespace AMI.Infrastructure.Services
{
    /// <summary>
    /// A service to handle the scheduling of cleanups.
    /// </summary>
    /// <seealso cref="ICleanupService" />
    public class CleanupService : ICleanupService
    {
        private readonly ILogger logger;
        private readonly IMediator mediator;
        private readonly IApiConfiguration configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="CleanupService"/> class.
        /// </summary>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="mediator">The mediator.</param>
        /// <param name="configuration">The API configuration.</param>
        public CleanupService(
            ILoggerFactory loggerFactory,
            IMediator mediator,
            IApiConfiguration configuration)
        {
            this.logger = loggerFactory?.CreateLogger<CleanupService>() ?? throw new ArgumentNullException(nameof(loggerFactory));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        /// <inheritdoc/>
        public async Task CleanupAsync(IWrappedJobCancellationToken ct)
        {
            Ensure.ArgumentNotNull(ct, nameof(ct));

            try
            {
                var cts = CancellationTokenSource.CreateLinkedTokenSource(ct.ShutdownToken);
                if (configuration.Options.CleanupPeriod > 0)
                {
                    cts.CancelAfter(configuration.Options.CleanupPeriod * 1000);

                    // TODO: cleanup events as well
                    var command = new ClearObjectsCommand()
                    {
                        RefDate = DateTime.Now.AddMinutes(-configuration.Options.CleanupPeriod)
                    };

                    await mediator.Send(command, cts.Token);
                }
            }
            catch (OperationCanceledException)
            {
                logger.LogInformation($"Cleanup canceled.");
            }
            catch (Exception e)
            {
                logger.LogWarning(e, $"Cleanup failed. {e.Message}");
            }
        }
    }
}
