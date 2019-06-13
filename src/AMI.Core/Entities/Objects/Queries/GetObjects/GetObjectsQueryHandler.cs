using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Entities.Models;
using AMI.Core.Repositories;
using MediatR;

namespace AMI.Core.Entities.Objects.Queries.GetObjects
{
    /// <summary>
    /// A handler for queries to get a list of paginated objects.
    /// </summary>
    public class GetObjectsQueryHandler : IRequestHandler<GetObjectsQuery, PaginationResultModel<ObjectModel>>
    {
        private readonly IAmiUnitOfWork context;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetObjectsQueryHandler"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <exception cref="ArgumentNullException">context</exception>
        public GetObjectsQueryHandler(IAmiUnitOfWork context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <inheritdoc/>
        public async Task<PaginationResultModel<ObjectModel>> Handle(GetObjectsQuery request, CancellationToken cancellationToken)
        {
            int total = await context.ObjectRepository.CountAsync(cancellationToken);

            var query = context.ObjectRepository
                .GetQuery()
                .OrderBy(e => e.CreatedDate)
                .Skip((request.Page - 1) * request.Limit)
                .Take(request.Limit);

            var entities = await context.ObjectRepository.ToListAsync(query);

            return PaginationResultModel<ObjectModel>.Create(entities.Select(e => ObjectModel.Create(e)), request.Page, request.Limit, total);
        }
    }
}
