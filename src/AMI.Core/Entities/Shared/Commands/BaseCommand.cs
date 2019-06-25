using System;
using System.Runtime.Serialization;
using AMI.Core.Entities.Results.Commands.ProcessObject;
using AMI.Core.Entities.Results.Commands.ProcessPath;
using AMI.Core.IO.Converters;
using AMI.Domain.Enums;
using Newtonsoft.Json;

namespace AMI.Core.Entities.Shared.Commands
{
    /// <summary>
    /// The base all commands have in common.
    /// </summary>
    [Serializable]
    [JsonConverter(typeof(JsonInheritanceConverter), "discriminator")]
    [KnownType(typeof(ProcessObjectCommand))]
    [KnownType(typeof(ProcessPathCommand))]
    public abstract class BaseCommand : IBaseCommand
    {
        /// <inheritdoc/>
        public abstract CommandType CommandType { get; }
    }
}
