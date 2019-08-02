using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Entities.Models;
using AMI.Core.Entities.Shared.Queries;
using AMI.Core.Modules;

namespace AMI.Core.Entities.Objects.Queries.GetObjects
{
    /// <summary>
    /// A query handler to get a list of paginated objects.
    /// </summary>
    public class GetObjectsQueryHandler : BaseQueryRequestHandler<GetObjectsQuery, PaginationResultModel<ObjectModel>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetObjectsQueryHandler"/> class.
        /// </summary>
        /// <param name="module">The query handler module.</param>
        public GetObjectsQueryHandler(IQueryHandlerModule module)
            : base(module)
        {
        }

        /// <inheritdoc/>
        protected override async Task<PaginationResultModel<ObjectModel>> ProtectedHandleAsync(GetObjectsQuery request, CancellationToken cancellationToken)
        {
            int total = await Context.ObjectRepository.CountAsync(cancellationToken);

            var query = Context.ObjectRepository
                .GetQuery()
                .Select(e => new
                {
                    Object = e,
                    Tasks = e.Tasks.OrderByDescending(e1 => e1.CreatedDate).Take(request.Limit),
                    Results = e.Tasks.OrderByDescending(e1 => e1.CreatedDate).Select(e1 => e1.Result).Take(request.Limit)
                })
                .OrderByDescending(e => e.Object.CreatedDate)
                .Skip(request.Page * request.Limit)
                .Take(request.Limit);

            var entities = await Context.ToListAsync(query, cancellationToken);
            var models = entities.Select(e => ObjectModel.Create(e.Object, e.Tasks, e.Results, Serializer));

            return PaginationResultModel<ObjectModel>.Create(models, request.Page, request.Limit, total);
        }
    }
}
