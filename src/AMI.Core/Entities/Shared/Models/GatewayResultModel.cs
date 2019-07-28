using AMI.Domain.Enums;

namespace AMI.Core.Entities.Models
{
    /// <summary>
    /// A model containing the result of a gateway notification.
    /// </summary>
    /// <typeparam name="T">The type of the data.</typeparam>
    public class GatewayResultModel<T>
    {
#pragma warning disable IDE1006, SA1300 // Element must begin with upper-case letter
        /// <summary>
        /// Gets or sets opcode for the payload.
        /// </summary>
        public GatewayOpCode op { get; set; }

        /// <summary>
        /// Gets or sets the event name for this payload.
        /// </summary>
        public GatewayEvent t { get; set; }

        /// <summary>
        /// Gets or sets event data.
        /// </summary>
        public T d { get; set; }
#pragma warning restore IDE1006, SA1300 // Element must begin with upper-case letter
    }
}
