using AMI.Core.Entities.Shared.Models;
using MediatR;

namespace AMI.Core.Entities.Shared.Queries.GetEntity
{
    /// <summary>
    /// A representation of a query to get an entity.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <seealso cref="IGetEntityQuery{T}" />
    /// <seealso cref="IRequest{T}" />
    public abstract class GetEntityQuery<T> : IGetEntityQuery<T>, IRequest<T>
        where T : IEntity
    {
        /// <inheritdoc/>
        public string Id { get; set; }
    }
}
