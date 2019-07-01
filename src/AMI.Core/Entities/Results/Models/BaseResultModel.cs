using System.Runtime.Serialization;
using AMI.Core.Entities.Shared.Models;
using AMI.Core.IO.Converters;
using AMI.Domain.Enums;
using Newtonsoft.Json;

namespace AMI.Core.Entities.Models
{
    /// <summary>
    /// The base all results have in common.
    /// </summary>
    [JsonConverter(typeof(JsonInheritanceConverter), "discriminator")]
    [KnownType(typeof(ProcessResultModel))]
    public abstract class BaseResultModel : IEntity
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets the type of the result.
        /// </summary>
        public abstract ResultType ResultType { get; }
    }
}
