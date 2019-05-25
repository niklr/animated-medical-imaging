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

        /// <summary>
        /// Initializes a new instance of the <see cref="UnexpectedNullException"/> class.
        /// </summary>
        /// <param name="baseEntityName">Name of the base entity.</param>
        /// <param name="nullEntityName">Name of the null entity.</param>
        public UnexpectedNullException(string baseEntityName, string nullEntityName)
            : base($"The entity \"{nullEntityName}\" of \"{baseEntityName}\" is null.")
        {
        }
    }
}
