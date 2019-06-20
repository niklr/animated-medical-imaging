using System;
using AMI.Core.Entities.Models;
using AMI.Core.Entities.Shared.Commands;
using AMI.Domain.Enums;

namespace AMI.Core.Entities.Results.Commands.ProcessObject
{
    /// <summary>
    /// A command containing information needed to process objects.
    /// </summary>
    [Serializable]
    public class ProcessObjectCommand : BaseProcessCommand<ProcessResultModel>
    {
        /// <inheritdoc/>
        public override CommandType CommandType => CommandType.ProcessObjectCommand;

        /// <summary>
        /// Gets or sets the identifier of the object.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the task.
        /// </summary>
        public string TaskId { get; set; }
    }
}
