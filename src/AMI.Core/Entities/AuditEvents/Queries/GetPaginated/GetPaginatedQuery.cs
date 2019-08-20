using AMI.Core.Entities.Models;
using AMI.Core.Entities.Shared.Queries.GetPaginated;

namespace AMI.Core.Entities.AuditEvents.Queries.GetPaginated
{
    /// <summary>
    /// An implementation of a query to get a list of paginated audit events.
    /// </summary>
    public class GetPaginatedQuery : GetPaginatedQuery<PaginationResultModel<AuditEventModel>>, IGetPaginatedQuery
    {
    }
}
