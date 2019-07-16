using System;
using MediatR;

namespace AMI.Core.Entities.Objects.Commands.Clear
{
    /// <summary>
    /// A command containing information needed to clear all objects.
    /// </summary>
    public class ClearObjectsCommand : IRequest<bool>
    {
        /// <summary>
        /// Gets or sets the reference date.
        /// </summary>
        public DateTime? RefDate { get; set; }
    }
}
