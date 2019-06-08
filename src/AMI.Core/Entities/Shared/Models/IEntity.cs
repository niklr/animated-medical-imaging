namespace AMI.Core.Entities.Shared.Models
{
    /// <summary>
    /// An interface for entities.
    /// </summary>
    public interface IEntity
    {
        /// <summary>
        /// Gets or sets the identifier of the entity.
        /// </summary>
        string Id { get; set; }
    }
}
