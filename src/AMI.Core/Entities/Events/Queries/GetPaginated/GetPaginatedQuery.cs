using AMI.Core.Entities.Models;
using AMI.Core.Entities.Shared.Queries.GetPaginated;

namespace AMI.Core.Entities.Events.Queries.GetPaginated
{
    /// <summary>
    /// An implementation of a query to get a list of paginated events.
    /// </summary>
    public class GetPaginatedQuery : GetPaginatedQuery<PaginationResultModel<EventModel>>, IGetPaginatedQuery
    {
    }
}
