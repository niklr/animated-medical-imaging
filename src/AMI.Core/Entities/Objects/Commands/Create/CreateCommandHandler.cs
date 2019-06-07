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
    /// <seealso cref="BaseCommandRequestHandler{CreateObjectCommand, ObjectModel}" />
    public class CreateCommandHandler : BaseCommandRequestHandler<CreateObjectCommand, ObjectModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateCommandHandler"/> class.
        /// </summary>
        /// <param name="mediator">The mediator.</param>
        public CreateCommandHandler(IMediator mediator)
            : base(mediator)
        {
        }

        /// <inheritdoc/>
        protected override Task<ObjectModel> ProtectedHandle(CreateObjectCommand request, CancellationToken cancellationToken)
        {
            return null;
        }
    }
}
