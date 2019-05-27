using System;

namespace AMI.Domain.Exceptions
{
    /// <summary>
    /// This exception is thrown when an operation was forbidden.
    /// </summary>
    /// <seealso cref="Exception" />
    public class ForbiddenException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ForbiddenException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public ForbiddenException(string message)
            : base($"Forbidden. {message}")
        {
        }
    }
}
