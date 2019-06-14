using System;
using AMI.Core.Entities.Models;
using AMI.Core.Entities.Shared.Commands;

namespace AMI.Core.Entities.Tasks.Commands.ProcessObjectAsync
{
    /// <summary>
    /// A command containing information needed to process objects.
    /// </summary>
    [Serializable]
    public class ProcessObjectAsyncCommand : BaseProcessCommand<ProcessObjectTaskModel>
    {
        /// <summary>
        /// Gets or sets the identifier of the object.
        /// </summary>
        public string Id { get; set; }
    }
}
