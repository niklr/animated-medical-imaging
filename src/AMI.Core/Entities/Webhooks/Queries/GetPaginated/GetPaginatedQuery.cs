using AMI.Core.Entities.Models;
using AMI.Core.Entities.Shared.Queries.GetPaginated;

namespace AMI.Core.Entities.Webhooks.Queries.GetPaginated
{
    /// <summary>
    /// An implementation of a query to get a list of paginated objects.
    /// </summary>
    public class GetPaginatedQuery : GetPaginatedQuery<PaginationResultModel<WebhookModel>>, IGetPaginatedQuery
    {
    }
}
