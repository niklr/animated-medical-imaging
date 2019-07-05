namespace AMI.Core.Entities.Models
{
    /// <summary>
    /// The API settings.
    /// </summary>
    public class ApiSettings
    {
        /// <summary>
        /// Gets or sets the name of header used to identify the IP address of the connecting client.
        /// </summary>
        public string ConnectingIpHeaderName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the current environment is development.
        /// </summary>
        public bool IsDevelopment { get; set;  }
    }
}
