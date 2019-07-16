using System;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Configurations;
using AMI.Core.Entities.Objects.Commands.Clear;
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
        private readonly ILogger logger;
        private readonly IMediator mediator;
        private readonly IApiConfiguration configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="CleanupRecurringWorker"/> class.
        /// </summary>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="mediator">The mediator.</param>
        /// <param name="configuration">The API configuration.</param>
        public CleanupRecurringWorker(ILoggerFactory loggerFactory, IMediator mediator, IApiConfiguration configuration)
            : base(loggerFactory)
        {
            logger = loggerFactory?.CreateLogger<TaskWorker>() ?? throw new ArgumentNullException(nameof(loggerFactory));
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
                await CleanupOnStartupAsync(cancellationToken);
                Schedule(cancellationToken);
            }

            await Task.CompletedTask;
        }

        private void Schedule(CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            if (configuration.Options.CleanupPeriod > 0)
            {
                NextActivityDate = DateTime.Now.AddMinutes(configuration.Options.CleanupPeriod);

                void action(DateTime date)
                {
                    Task.Run(async () => await CleanupAsync(ct));
                    Schedule(ct);
                }

                // need to hold on to the reference to the timer otherwise the timer object will be garbage collected, which will run its finalizer, stopping the timer.
                Timer timer = SimpleScheduler.CallActionAt(NextActivityDate, action, NextActivityDate);
                Timer = timer;
            }
        }

        private async Task CleanupOnStartupAsync(CancellationToken ct)
        {
            try
            {
                ct.ThrowIfCancellationRequested();

                await mediator.Send(new ClearObjectsCommand(), ct);
            }
            catch (OperationCanceledException)
            {
                logger.LogInformation($"Cleanup on startup {WorkerName} canceled.");
            }
            catch (Exception e)
            {
                logger.LogWarning(e, $"Cleanup on startup {WorkerName} failed. {e.Message}");
            }
        }

        private async Task CleanupAsync(CancellationToken ct)
        {
            try
            {
                ct.ThrowIfCancellationRequested();

                var command = new ClearObjectsCommand()
                {
                    RefDate = NextActivityDate
                };

                await mediator.Send(command, ct);
            }
            catch (OperationCanceledException)
            {
                logger.LogInformation($"Cleanup {WorkerName} canceled.");
            }
            catch (Exception e)
            {
                logger.LogWarning(e, $"Cleanup {WorkerName} failed. {e.Message}");
            }
        }
    }
}
