using System;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Entities.Models;
using AMI.Core.Entities.Results.Commands.ProcessPath;
using AMI.Core.Entities.Shared.Commands;
using AMI.Core.IO.Serializers;
using AMI.Core.Repositories;
using AMI.Core.Services;
using AMI.Domain.Exceptions;
using MediatR;

namespace AMI.Core.Entities.Results.Commands.ProcessObject
{
    /// <summary>
    /// A handler for process command requests.
    /// </summary>
    public class ProcessCommandHandler : BaseCommandRequestHandler<ProcessObjectCommand, ProcessResultModel>
    {
        private readonly IAmiUnitOfWork context;
        private readonly IIdGenService idGenService;
        private readonly IDefaultJsonSerializer serializer;
        private readonly IMediator mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessCommandHandler"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="idGenService">The service to generate unique identifiers.</param>
        /// <param name="serializer">The JSON serializer.</param>
        /// <param name="mediator">The mediator.</param>
        public ProcessCommandHandler(
            IAmiUnitOfWork context,
            IIdGenService idGenService,
            IDefaultJsonSerializer serializer,
            IMediator mediator)
            : base()
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.idGenService = idGenService ?? throw new ArgumentNullException(nameof(idGenService));
            this.serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
            this.mediator = this.mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        /// <inheritdoc/>
        protected override async Task<ProcessResultModel> ProtectedHandleAsync(ProcessObjectCommand request, CancellationToken cancellationToken)
        {
            var pathRequest = new ProcessPathCommand()
            {
                
            };

            // TODO: extract if needed before processing
            var result = await mediator.Send(pathRequest, cancellationToken);

            if (result == null)
            {
                throw new UnexpectedNullException($"The processing result of object {request.Id} was null.");
            }

            // TODO: update JsonFsPath
            return result;
        }
    }
}
