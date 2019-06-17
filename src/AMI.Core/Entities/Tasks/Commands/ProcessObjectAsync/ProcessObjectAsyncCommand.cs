using System;
using AMI.Core.Entities.Models;
using AMI.Core.Entities.Shared.Commands;
using AMI.Domain.Enums;

namespace AMI.Core.Entities.Tasks.Commands.ProcessObjectAsync
{
    /// <summary>
    /// A command containing information needed to process objects.
    /// </summary>
    [Serializable]
    public class ProcessObjectAsyncCommand : BaseProcessCommand<TaskModel>
    {
        /// <inheritdoc/>
        public override CommandType CommandType => CommandType.ProcessObjectAsyncCommand;

        /// <summary>
        /// Gets or sets the identifier of the object.
        /// </summary>
        public string Id { get; set; }
    }
}
