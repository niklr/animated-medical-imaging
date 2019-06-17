using System.Runtime.Serialization;
using AMI.Domain.Enums;

namespace AMI.Core.Entities.Models
{
    /// <summary>
    /// The base all results have in common.
    /// </summary>
    [KnownType(typeof(ProcessResultModel))]
    public abstract class BaseResultModel
    {
        /// <summary>
        /// Gets the type of the result.
        /// </summary>
        public abstract ResultType ResultType { get; }
    }
}
