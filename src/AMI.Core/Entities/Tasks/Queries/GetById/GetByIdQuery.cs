using AMI.Core.Entities.Models;
using AMI.Core.Entities.Shared.Models;
using AMI.Core.Entities.Shared.Queries.GetEntity;

namespace AMI.Core.Entities.Tasks.Queries.GetById
{
    /// <summary>
    /// An implementation of a query to get an entity by its identifier.
    /// </summary>
    public class GetByIdQuery : GetEntityQuery<TaskModel>, IGetEntityQuery<IEntity>
    {
    }
}
