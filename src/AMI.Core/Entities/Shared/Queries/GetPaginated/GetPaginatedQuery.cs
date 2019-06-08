using MediatR;

namespace AMI.Core.Entities.Shared.Queries.GetPaginated
{
    /// <summary>
    /// A representation of a query to get a paginated list of entities.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    public abstract class GetPaginatedQuery<T> : IGetPaginatedQuery, IRequest<T>
    {
        /// <inheritdoc/>
        public int Page { get; set; }

        /// <inheritdoc/>
        public int Limit { get; set; }
    }
}
