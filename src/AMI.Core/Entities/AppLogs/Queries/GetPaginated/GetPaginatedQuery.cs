using AMI.Core.Entities.Models;
using AMI.Core.Entities.Shared.Queries.GetPaginated;

namespace AMI.Core.Entities.AppLogs.Queries.GetPaginated
{
    /// <summary>
    /// An implementation of a query to get a list of paginated application logs.
    /// </summary>
    public class GetPaginatedQuery : GetPaginatedQuery<PaginationResultModel<AppLogModel>>, IGetPaginatedQuery
    {
    }
}
