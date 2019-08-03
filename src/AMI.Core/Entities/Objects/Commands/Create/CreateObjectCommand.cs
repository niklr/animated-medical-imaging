using AMI.Core.Entities.Models;
using MediatR;

namespace AMI.Core.Entities.Objects.Commands.Create
{
    /// <summary>
    /// A command containing information needed for object creation.
    /// </summary>
    public class CreateObjectCommand : IRequest<ObjectModel>
    {
        /// <summary>
        /// Gets or sets the original filename.
        /// </summary>
        public string OriginalFilename { get; set; }

        /// <summary>
        /// Gets or sets the source path.
        /// </summary>
        public string SourcePath { get; set; }
    }
}
