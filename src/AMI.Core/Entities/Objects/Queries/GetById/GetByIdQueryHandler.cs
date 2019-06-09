using System;
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
    /// A handler for queries to get an object by its identifier.
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
            var result = await uow.ObjectRepository
                .GetFirstOrDefaultAsync(e => e.Id == Guid.Parse(request.Id), cancellationToken);

            if (result == null)
            {
                throw new NotFoundException(nameof(ObjectEntity), request.Id);
            }

            return ObjectModel.Create(result);
        }
    }
}
