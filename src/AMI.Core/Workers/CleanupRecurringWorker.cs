using System;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Configurations;
using AMI.Core.Entities.Objects.Commands.Clear;
using AMI.Core.Services;
using AMI.Domain.Enums;
using MediatR;
using Microsoft.Extensions.Logging;
using RNS.Framework.Threading;

namespace AMI.Core.Workers
{
    /// <summary>
    /// A recurring worker to cleanup the working directory.
    /// </summary>
    /// <seealso cref="RecurringWorker" />
    public class CleanupRecurringWorker : RecurringWorker
    {
        private readonly IMediator mediator;
        private readonly IApiConfiguration configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="CleanupRecurringWorker"/> class.
        /// </summary>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="workerService">The worker service.</param>
        /// <param name="gateway">The gateway service.</param>
        /// <param name="mediator">The mediator.</param>
        /// <param name="configuration">The API configuration.</param>
        public CleanupRecurringWorker(
            ILoggerFactory loggerFactory,
            IWorkerService workerService,
            IGatewayService gateway,
            IMediator mediator,
            IApiConfiguration configuration)
            : base(loggerFactory, workerService, gateway)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        /// <inheritdoc/>
        public override WorkerType WorkerType => WorkerType.Recurring;

        /// <inheritdoc/>
        protected override async Task DoWorkAsync(CancellationToken cancellationToken)
        {
            if (cancellationToken == null)
            {
                throw new ArgumentNullException(nameof(cancellationToken));
            }

            if (configuration.Options.CleanupPeriod > 0)
            {
                Schedule(cancellationToken);
            }

            await Task.CompletedTask;
        }

        private void Schedule(CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            if (configuration.Options.CleanupPeriod > 0)
            {
                DateTime refDate = DateTime.Now.AddMinutes(-configuration.Options.CleanupPeriod);
                NextActivityDate = DateTime.Now.AddMinutes(1);

                void action(DateTime date)
                {
                    Task.Run(async () => await CleanupAsync(date, ct));
                    Schedule(ct);
                }

                Timer = SimpleScheduler.CallActionAt(NextActivityDate, action, refDate);
            }
        }

        private async Task CleanupAsync(DateTime refDate, CancellationToken ct)
        {
            try
            {
                ct.ThrowIfCancellationRequested();

                var command = new ClearObjectsCommand()
                {
                    RefDate = refDate
                };

                await mediator.Send(command, ct);
            }
            catch (OperationCanceledException)
            {
                Logger.LogInformation($"Cleanup {WorkerName} canceled.");
            }
            catch (Exception e)
            {
                Logger.LogWarning(e, $"Cleanup {WorkerName} failed. {e.Message}");
            }
        }
    }
}
