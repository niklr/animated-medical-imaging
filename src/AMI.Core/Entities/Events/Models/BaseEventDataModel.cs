using System.Runtime.Serialization;
using AMI.Core.IO.Converters;
using Newtonsoft.Json;

namespace AMI.Core.Entities.Models
{
    /// <summary>
    /// The base properties all event data have in common.
    /// </summary>
    [JsonConverter(typeof(JsonInheritanceConverter), "discriminator")]
    [KnownType(typeof(TaskEventDataModel))]
    [KnownType(typeof(ObjectEventDataModel))]
    [KnownType(typeof(AuditEventDataModel))]
    public abstract class BaseEventDataModel
    {
    }
}
