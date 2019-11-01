using System;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Entities.Models;
using AMI.Core.Entities.Shared.Queries;
using AMI.Core.Modules;
using AMI.Domain.Entities;
using AMI.Domain.Exceptions;

namespace AMI.Core.Entities.Users.Queries.GetById
{
    /// <summary>
    /// A query handler to get an entity by its identifier.
    /// </summary>
    public class GetByIdQueryHandler : BaseQueryRequestHandler<GetByIdQuery, UserModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetByIdQueryHandler"/> class.
        /// </summary>
        /// <param name="module">The query handler module.</param>
        public GetByIdQueryHandler(IQueryHandlerModule module)
            : base(module)
        {
        }

        /// <inheritdoc/>
        protected override async Task<UserModel> ProtectedHandleAsync(GetByIdQuery request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var result = await Context.UserRepository.GetFirstOrDefaultAsync(e => e.Id == Guid.Parse(request.Id), cancellationToken);

            if (result == null)
            {
                throw new NotFoundException(nameof(ObjectEntity), request.Id);
            }

            var model = UserModel.Create(result, Constants);

            return model;
        }
    }
}
