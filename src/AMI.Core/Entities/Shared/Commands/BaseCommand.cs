using System;
using System.Runtime.Serialization;
using AMI.Core.Entities.Results.Commands.ProcessObject;
using AMI.Core.Entities.Results.Commands.ProcessPath;
using AMI.Core.Entities.Tasks.Commands.ProcessObjectAsync;
using AMI.Domain.Enums;

namespace AMI.Core.Entities.Shared.Commands
{
    /// <summary>
    /// The base all commands have in common.
    /// </summary>
    [Serializable]
    [KnownType(typeof(ProcessObjectCommand))]
    [KnownType(typeof(ProcessObjectAsyncCommand))]
    [KnownType(typeof(ProcessPathCommand))]
    public abstract class BaseCommand : IBaseCommand
    {
        /// <inheritdoc/>
        public abstract CommandType CommandType { get; }
    }
}
