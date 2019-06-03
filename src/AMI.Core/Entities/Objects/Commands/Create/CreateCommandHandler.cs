using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Entities.Models;
using AMI.Core.Entities.Shared.Commands;
using MediatR;

namespace AMI.Core.Entities.Objects.Commands.Create
{
    /// <summary>
    /// A handler for create command requests.
    /// </summary>
    /// <seealso cref="BaseCommandRequestHandler{CreateObjectCommand, ObjectResult}" />
    public class CreateCommandHandler : BaseCommandRequestHandler<CreateObjectCommand, ObjectResult>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateCommandHandler"/> class.
        /// </summary>
        /// <param name="mediator">The mediator.</param>
        public CreateCommandHandler(IMediator mediator)
            : base(mediator)
        {
        }

        /// <summary>
        /// Handles the command request called by the base class.
        /// </summary>
        /// <param name="request">The command request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The result of the command request.</returns>
        protected override Task<ObjectResult> ProtectedHandle(CreateObjectCommand request, CancellationToken cancellationToken)
        {
            return null;
        }
    }
}
