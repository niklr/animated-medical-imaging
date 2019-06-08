namespace AMI.Core.Entities.Shared.Queries.GetPaginated
{
    /// <summary>
    /// An interface for queries to get a paginated list of entities.
    /// </summary>
    public interface IGetPaginatedQuery
    {
        /// <summary>
        /// Gets or sets the page.
        /// </summary>
        int Page { get; set; }

        /// <summary>
        /// Gets or sets the limit.
        /// </summary>
        int Limit { get; set; }
    }
}
