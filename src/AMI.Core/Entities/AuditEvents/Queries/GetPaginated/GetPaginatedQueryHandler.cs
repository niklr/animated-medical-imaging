using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Entities.Models;
using AMI.Core.Entities.Shared.Queries;
using AMI.Core.Modules;

namespace AMI.Core.Entities.AuditEvents.Queries.GetPaginated
{
    /// <summary>
    /// A query handler to get a list of paginated audit events.
    /// </summary>
    public class GetPaginatedQueryHandler : BaseQueryRequestHandler<GetPaginatedQuery, PaginationResultModel<AuditEventModel>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetPaginatedQueryHandler"/> class.
        /// </summary>
        /// <param name="module">The query handler module.</param>
        public GetPaginatedQueryHandler(IQueryHandlerModule module)
            : base(module)
        {
        }

        /// <inheritdoc/>
        protected override async Task<PaginationResultModel<AuditEventModel>> ProtectedHandleAsync(GetPaginatedQuery request, CancellationToken cancellationToken)
        {
            int total = await Context.AuditEventRepository.CountAsync(cancellationToken);

            var result = Context.AuditEventRepository
                .GetQuery()
                .OrderByDescending(e => e.Timestamp)
                .Skip(request.Page * request.Limit)
                .Take(request.Limit)
                .Select(e => AuditEventModel.Create(e, Serializer));

            return PaginationResultModel<AuditEventModel>.Create(result, request.Page, request.Limit, total);
        }
    }
}
