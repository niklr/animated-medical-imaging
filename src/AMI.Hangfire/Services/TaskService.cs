using System;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Configurations;
using AMI.Core.Entities.Models;
using AMI.Core.Entities.Results.Commands.ProcessObject;
using AMI.Core.Entities.Tasks.Commands.UpdateStatus;
using AMI.Core.Entities.Tasks.Queries.GetById;
using AMI.Domain.Enums;
using AMI.Domain.Exceptions;
using Hangfire;
using MediatR;
using Microsoft.Extensions.Logging;
using RNS.Framework.Tools;

namespace AMI.Hangfire.Services
{
    /// <summary>
    /// A service to handle tasks related to background processing.
    /// </summary>
    public class TaskService : ITaskService
    {
        private readonly ILogger logger;
        private readonly IMediator mediator;
        private readonly IAppConfiguration configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskService"/> class.
        /// </summary>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="mediator">The mediator.</param>
        /// <param name="configuration">The application configuration.</param>
        public TaskService(
            ILoggerFactory loggerFactory,
            IMediator mediator,
            IAppConfiguration configuration)
        {
            this.logger = loggerFactory?.CreateLogger<TaskService>() ?? throw new ArgumentNullException(nameof(loggerFactory));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        /// <inheritdoc/>
        public async Task ProcessAsync(string id, IJobCancellationToken ct)
        {
            Ensure.ArgumentNotNullOrWhiteSpace(id, nameof(id));
            Ensure.ArgumentNotNull(ct, nameof(ct));

            // TODO: use IJobCancellationToken
            var cts = new CancellationTokenSource();
            if (configuration?.Options?.TimeoutMilliseconds > 0)
            {
                cts.CancelAfter(configuration.Options.TimeoutMilliseconds);
            }

            try
            {
                var task = await mediator.Send(new GetByIdQuery { Id = id });
                if (task == null)
                {
                    throw new UnexpectedNullException("Task not found.");
                }

                if (task.Command == null)
                {
                    await UpdateStatus(mediator, task, Domain.Enums.TaskStatus.Finished, string.Empty);
                }
                else
                {
                    await UpdateStatus(mediator, task, Domain.Enums.TaskStatus.Processing, string.Empty);

                    switch (task.Command.CommandType)
                    {
                        case CommandType.ProcessObjectCommand:
                            await ProcessObjectAsync(mediator, task, cts.Token);
                            break;
                        default:
                            await UpdateStatus(mediator, task, Domain.Enums.TaskStatus.Finished, string.Empty);
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                // Task status could not be updated.
                logger.LogCritical(e, e.Message);
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
                logger.LogInformation($"Processing of task {item.Id} canceled.");
                await UpdateStatus(mediator, item, Domain.Enums.TaskStatus.Canceled, string.Empty);
            }
            catch (Exception e)
            {
                logger.LogWarning(e, $"Processing of task {item.Id} failed. {e.Message}");
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
    }
}
