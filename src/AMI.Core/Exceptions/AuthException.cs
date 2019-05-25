using System;

namespace PNL.Application.Exceptions
{
    /// <summary>
    /// This exception is thrown when an exception related to authentication or authorization occurs.
    /// </summary>
    /// <seealso cref="Exception" />
    public class AuthException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public AuthException(string message)
            : base($"Authentication failed. {message}")
        {
        }
    }
}
