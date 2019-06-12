using AMI.Core.Entities.Models;
using MediatR;

namespace AMI.Core.Entities.Objects.Commands.Delete
{
    /// <summary>
    /// A command containing information needed for object deletion.
    /// </summary>
    /// <seealso cref="IRequest{ObjectModel}" />
    public class DeleteObjectCommand : IRequest<ObjectModel>
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        public string Id { get; set; }
    }
}
