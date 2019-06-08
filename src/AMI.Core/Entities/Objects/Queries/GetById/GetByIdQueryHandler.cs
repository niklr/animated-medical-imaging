using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Entities.Models;
using AMI.Core.Repositories;
using AMI.Domain.Entities;
using AMI.Domain.Exceptions;
using MediatR;

namespace AMI.Core.Entities.Objects.Queries.GetById
{
    /// <summary>
    /// A handler for a query to get an object by it's identifier.
    /// </summary>
    public class GetByIdQueryHandler : IRequestHandler<GetByIdQuery, ObjectModel>
    {
        private readonly IAmiUnitOfWork uow;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetByIdQueryHandler"/> class.
        /// </summary>
        /// <param name="uow">The context.</param>
        public GetByIdQueryHandler(IAmiUnitOfWork uow)
        {
            this.uow = uow;
        }

        /// <inheritdoc/>
        public async Task<ObjectModel> Handle(GetByIdQuery request, CancellationToken cancellationToken)
        {
            // TODO: Implement async
            // .SingleOrDefaultAsync(cancellationToken);
            var result = uow.ObjectRepository
                .GetQuery(e => e.Id == Guid.Parse(request.Id))
                .Select(ObjectModel.Projection)
                .FirstOrDefault();

            if (result == null)
            {
                throw new NotFoundException(nameof(ObjectVersion), request.Id);
            }

            return result;
        }
    }
}
