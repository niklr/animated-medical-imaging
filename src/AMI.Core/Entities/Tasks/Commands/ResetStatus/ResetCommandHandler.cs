using System;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Entities.Models;
using AMI.Core.Entities.Shared.Commands;
using AMI.Core.Entities.Tasks.Queries.GetById;
using AMI.Core.IO.Serializers;
using AMI.Core.Modules;
using AMI.Domain.Entities;
using AMI.Domain.Enums;
using AMI.Domain.Exceptions;
using MediatR;

namespace AMI.Core.Entities.Tasks.Commands.ResetStatus
{
    /// <summary>
    /// A handler for command requests to update the status of a task.
    /// </summary>
    public class ResetCommandHandler : BaseCommandRequestHandler<ResetTaskStatusCommand, bool>
    {
        private readonly IDefaultJsonSerializer serializer;
        private readonly IMediator mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResetCommandHandler"/> class.
        /// </summary>
        /// <param name="module">The command handler module.</param>
        /// <param name="serializer">The JSON serializer.</param>
        /// <param name="mediator">The mediator.</param>
        public ResetCommandHandler(
            ICommandHandlerModule module,
            IDefaultJsonSerializer serializer,
            IMediator mediator)
            : base(module)
        {
            this.serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        /// <inheritdoc/>
        protected override async Task<bool> ProtectedHandleAsync(ResetTaskStatusCommand request, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;

            return true;
        }
    }
}
