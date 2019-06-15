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
            var limit = 25;
            var result = uow.ObjectRepository
                .GetQuery()
                .Where(e => e.Id == Guid.Parse(request.Id))
                .Select(e => new
                {
                    Object = e,
                    Tasks = e.Tasks.OrderByDescending(e1 => e1.CreatedDate).Take(limit),
                    Results = e.Tasks.OrderByDescending(e1 => e1.CreatedDate).Select(e1 => e1.Result).Take(limit)
                })
                .FirstOrDefault();

            if (result == null)
            {
                throw new NotFoundException(nameof(ObjectEntity), request.Id);
            }

            return await Task.Run(() => { return ObjectModel.Create(result.Object); });
        }
    }
}
