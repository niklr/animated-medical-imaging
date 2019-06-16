using System;
using System.Runtime.Serialization;
using AMI.Core.Entities.Results.Commands.ProcessObject;
using AMI.Core.Entities.Results.Commands.ProcessPath;
using AMI.Core.Entities.Tasks.Commands.ProcessObjectAsync;

namespace AMI.Core.Entities.Shared.Commands
{
    /// <summary>
    /// A command containing information used to create a task.
    /// </summary>
    [Serializable]
    [KnownType(typeof(ProcessObjectCommand))]
    [KnownType(typeof(ProcessObjectAsyncCommand))]
    [KnownType(typeof(ProcessPathCommand))]
    public abstract class BaseTaskCommand
    {
    }
}
