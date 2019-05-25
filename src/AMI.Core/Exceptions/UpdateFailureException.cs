using System;

namespace AMI.Core.Exceptions
{
    /// <summary>
    /// This exception is thrown when an update failure occurs.
    /// </summary>
    /// <seealso cref="Exception" />
    public class UpdateFailureException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateFailureException"/> class.
        /// </summary>
        /// <param name="name">The name of the entity.</param>
        /// <param name="key">The key of the entity.</param>
        /// <param name="message">The message that describes the error.</param>
        public UpdateFailureException(string name, object key, string message)
            : base($"Update of entity \"{name}\" ({key}) failed. {message}")
        {
        }
    }
}
