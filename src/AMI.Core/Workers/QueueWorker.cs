using System;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Configurations;
using AMI.Core.Entities.Models;
using AMI.Core.Entities.Results.Commands.ProcessObject;
using AMI.Core.Entities.Tasks.Commands.UpdateStatus;
using AMI.Core.Queues;
using AMI.Core.Services;
using AMI.Domain.Enums;
using AMI.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMI.Core.Workers
{
    /// <summary>
    /// A worker to process queues.
    /// </summary>
    /// <seealso cref="BaseWorker" />
    public class QueueWorker : BaseWorker, IQueueWorker
    {
        private readonly IAppConfiguration configuration;
        private readonly ITaskQueue queue;
        private readonly IServiceProvider serviceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="QueueWorker"/> class.
        /// </summary>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="workerService">The worker service.</param>
        /// <param name="configuration">The application configuration.</param>
        /// <param name="queue">The queue.</param>
        /// <param name="serviceProvider">The service provider.</param>
        public QueueWorker(ILoggerFactory loggerFactory, IWorkerService workerService, IAppConfiguration configuration, ITaskQueue queue, IServiceProvider serviceProvider)
            : base(loggerFactory, workerService)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.queue = queue ?? throw new ArgumentNullException(nameof(queue));
            this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        /// <inheritdoc/>
        public override WorkerType WorkerType => WorkerType.Queue;

        /// <inheritdoc/>
        public int Count => queue.Count;

        /// <inheritdoc/>
        protected override async Task DoWorkAsync(CancellationToken ct)
        {
            foreach (var item in queue.GetConsumingEnumerable(ct))
            {
                ct.ThrowIfCancellationRequested();

                StartWatch();

                var cts = CancellationTokenSource.CreateLinkedTokenSource(ct);
                if (configuration?.Options?.TimeoutMilliseconds > 0)
                {
                    cts.CancelAfter(configuration.Options.TimeoutMilliseconds);
                }

                try
                {
                    IMediator mediator = serviceProvider.GetService<IMediator>();
                    if (mediator == null)
                    {
                        throw new UnexpectedNullException("The mediator could not be resolved.");
                    }

                    if (item.Command == null)
                    {
                        await UpdateStatus(mediator, item, Domain.Enums.TaskStatus.Finished, string.Empty);
                        await UpdatePositionsAsync();
                    }
                    else
                    {
                        await UpdateStatus(mediator, item, Domain.Enums.TaskStatus.Processing, string.Empty);
                        await UpdatePositionsAsync();

                        switch (item.Command.CommandType)
                        {
                            case CommandType.ProcessObjectCommand:
                                await ProcessObjectAsync(mediator, item, cts.Token);
                                break;
                            default:
                                await UpdateStatus(mediator, item, Domain.Enums.TaskStatus.Finished, string.Empty);
                                break;
                        }
                    }
                }
                catch (Exception e)
                {
                    // Task status could not be updated.
                    Logger.LogCritical(e, e.Message);
                }

                StopWatch();
            }
        }

        private async Task ProcessObjectAsync(IMediator mediator, TaskModel item, CancellationToken ct)
        {
            try
            {
                if (item.Command == null)
                {
                    throw new UnexpectedNullException($"The command of task {item.Id} is null.");
                }

                if (item.Command.GetType() != typeof(ProcessObjectCommand))
                {
                    throw new AmiException($"Task {item.Id} has an unexpected command type.");
                }

                var result = await mediator.Send((ProcessObjectCommand)item.Command, ct);

                await UpdateStatus(mediator, item, result.Id, Domain.Enums.TaskStatus.Finished, string.Empty);
            }
            catch (OperationCanceledException)
            {
                Logger.LogInformation($"Processing of task {item.Id} canceled.");
                await UpdateStatus(mediator, item, Domain.Enums.TaskStatus.Canceled, string.Empty);
            }
            catch (Exception e)
            {
                Logger.LogWarning(e, $"Processing of task {item.Id} failed. {e.Message}");
                await UpdateStatus(mediator, item, Domain.Enums.TaskStatus.Failed, e.Message);
            }
        }

        private async Task UpdateStatus(IMediator mediator, TaskModel task, Domain.Enums.TaskStatus status, string message)
        {
            await UpdateStatus(mediator, task, string.Empty, status, message);
        }

        private async Task UpdateStatus(IMediator mediator, TaskModel task, string resultId, Domain.Enums.TaskStatus status, string message)
        {
            var command = new UpdateTaskStatusCommand()
            {
                Id = task.Id,
                ResultId = resultId,
                Status = status,
                Message = message
            };

            await mediator.Send(command);
        }

        private async Task UpdatePositionsAsync()
        {
            // TODO: Decrease all positions by 1
            await Task.CompletedTask;
        }
    }
}
