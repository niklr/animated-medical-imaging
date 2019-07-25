using MediatR;

namespace AMI.Core.Entities.Objects.Commands.Delete
{
    /// <summary>
    /// A command containing information needed for object deletion.
    /// </summary>
    public class DeleteObjectCommand : IRequest<bool>
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        public string Id { get; set; }
    }
}
