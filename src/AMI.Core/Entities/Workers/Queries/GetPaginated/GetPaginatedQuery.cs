using AMI.Core.Entities.Models;
using AMI.Core.Entities.Shared.Queries.GetPaginated;

namespace AMI.Core.Entities.Workers.Queries.GetPaginated
{
    /// <summary>
    /// An implementation of a query to get a list of paginated workers.
    /// </summary>
    public class GetPaginatedQuery : GetPaginatedQuery<PaginationResultModel<BaseWorkerModel>>, IGetPaginatedQuery
    {
    }
}
