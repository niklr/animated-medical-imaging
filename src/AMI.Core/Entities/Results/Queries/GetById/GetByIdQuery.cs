using AMI.Core.Entities.Models;
using AMI.Core.Entities.Shared.Models;
using AMI.Core.Entities.Shared.Queries.GetEntity;

namespace AMI.Core.Entities.Results.Queries.GetById
{
    /// <summary>
    /// An implementation of a query to get a result by its identifier.
    /// </summary>
    public class GetByIdQuery : GetEntityQuery<ResultModel>, IGetEntityQuery<IEntity>
    {
    }
}
