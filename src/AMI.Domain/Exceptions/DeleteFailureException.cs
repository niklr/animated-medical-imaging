using System;

namespace AMI.Domain.Exceptions
{
    /// <summary>
    /// This exception is thrown when a deletion operation failed.
    /// </summary>
    /// <seealso cref="Exception" />
    public class DeleteFailureException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteFailureException"/> class.
        /// </summary>
        /// <param name="name">The name of the entity.</param>
        /// <param name="key">The key of the entity.</param>
        /// <param name="message">The message that describes the error.</param>
        public DeleteFailureException(string name, object key, string message)
            : base($"Deletion of entity \"{name}\" ({key}) failed. {message}")
        {
        }
    }
}
