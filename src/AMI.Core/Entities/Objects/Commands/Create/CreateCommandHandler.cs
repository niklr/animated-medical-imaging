using System;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Entities.Models;
using AMI.Core.Entities.Shared.Commands;
using AMI.Core.Repositories;
using AMI.Core.Services;
using AMI.Domain.Entities;

namespace AMI.Core.Entities.Objects.Commands.Create
{
    /// <summary>
    /// A handler for create command requests.
    /// </summary>
    /// <seealso cref="BaseCommandRequestHandler{CreateObjectCommand, ObjectModel}" />
    public class CreateCommandHandler : BaseCommandRequestHandler<CreateObjectCommand, ObjectModel>
    {
        private readonly IAmiUnitOfWork context;
        private readonly IIdGenService idGenService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateCommandHandler"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="idGenService">The service to generate unique identifiers.</param>
        public CreateCommandHandler(IAmiUnitOfWork context, IIdGenService idGenService)
            : base()
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.idGenService = idGenService ?? throw new ArgumentNullException(nameof(idGenService));
        }

        /// <inheritdoc/>
        protected override async Task<ObjectModel> ProtectedHandleAsync(CreateObjectCommand request, CancellationToken cancellationToken)
        {
            var entity = new ObjectVersion()
            {
                Id = idGenService.CreateId(),
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow,
                OriginalFilename = request.OriginalFilename,
                SourcePath = request.SourcePath
            };

            context.ObjectRepository.Add(entity);

            await context.SaveChangesAsync(cancellationToken);

            return ObjectModel.Create(entity);
        }
    }
}
