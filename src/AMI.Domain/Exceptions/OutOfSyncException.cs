using System;

namespace AMI.Domain.Exceptions
{
    /// <summary>
    /// This exception is thrown when an operation is out of sync.
    /// </summary>
    /// <seealso cref="Exception" />
    public class OutOfSyncException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OutOfSyncException"/> class.
        /// </summary>
        /// <param name="e">The exception.</param>
        public OutOfSyncException(Exception e)
            : base("The operation failed probably because the client is out of sync.", e)
        {
        }
    }
}
