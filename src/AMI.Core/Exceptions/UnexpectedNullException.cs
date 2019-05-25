using System;

namespace PNL.Application.Exceptions
{
    /// <summary>
    /// This exception is thrown when an entity is unexpectedly null.
    /// </summary>
    /// <seealso cref="Exception" />
    public class UnexpectedNullException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnexpectedNullException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public UnexpectedNullException(string message)
            : base($"Unexpected null exception. {message}")
        {
        }
    }
}
