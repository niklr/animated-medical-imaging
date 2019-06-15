using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Entities.Models;
using AMI.Core.IO.Serializers;
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
        private readonly IDefaultJsonSerializer serializer;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetObjectsQueryHandler"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="serializer">The JSON serializer.</param>
        public GetObjectsQueryHandler(IAmiUnitOfWork context, IDefaultJsonSerializer serializer)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
        }

        /// <inheritdoc/>
        public async Task<PaginationResultModel<ObjectModel>> Handle(GetObjectsQuery request, CancellationToken cancellationToken)
        {
            int total = await context.ObjectRepository.CountAsync(cancellationToken);

            var entities = context.ObjectRepository
                .GetQuery()
                .Select(e => new
                {
                    Object = e,
                    Tasks = e.Tasks.OrderByDescending(e1 => e1.CreatedDate).Take(request.Limit),
                    Results = e.Tasks.OrderByDescending(e1 => e1.CreatedDate).Select(e1 => e1.Result).Take(request.Limit)
                })
                .OrderByDescending(e => e.Object.CreatedDate)
                .Skip((request.Page - 1) * request.Limit)
                .Take(request.Limit)
                .ToList()
                .Select(e => ObjectModel.Create(e.Object, e.Tasks, e.Results, serializer));

            return PaginationResultModel<ObjectModel>.Create(entities, request.Page, request.Limit, total);
        }
    }
}
