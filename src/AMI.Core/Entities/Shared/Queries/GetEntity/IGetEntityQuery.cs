namespace AMI.Core.Entities.Shared.Queries.GetEntity
{
    /// <summary>
    /// An interface for a query to get an entity.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    public interface IGetEntityQuery<T>
    {
        /// <summary>
        /// Gets or sets the identifier of the entity.
        /// </summary>
        string Id { get; set; }
    }
}
