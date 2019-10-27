using AMI.Domain.Enums;

namespace AMI.Core.Entities.Models
{
    /// <summary>
    /// A model containing the result of a gateway notification.
    /// </summary>
    /// <typeparam name="T">The type of the data.</typeparam>
    public class GatewayResultModel<T>
    {
        /// <summary>
        /// Gets or sets the event name for this payload.
        /// </summary>
        public EventType EventType { get; set; }

        /// <summary>
        /// Gets or sets event data.
        /// </summary>
        public T Data { get; set; }
    }
}
