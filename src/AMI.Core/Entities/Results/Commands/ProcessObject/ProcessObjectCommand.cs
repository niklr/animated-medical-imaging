using System;
using AMI.Core.Entities.Shared.Commands;

namespace AMI.Core.Entities.Results.Commands.ProcessObjects
{
    /// <summary>
    /// A command containing information needed to process objects.
    /// </summary>
    [Serializable]
    public class ProcessObjectCommand : BaseProcessCommand
    {
        /// <summary>
        /// Gets or sets the identifier of the object.
        /// </summary>
        public string Id { get; set; }
    }
}
