using System;

namespace AMI.Core.Exceptions
{
    /// <summary>
    /// This exception is thrown when an entity was not found.
    /// </summary>
    /// <seealso cref="Exception" />
    public class NotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NotFoundException"/> class.
        /// </summary>
        /// <param name="name">The name of the entity.</param>
        /// <param name="key">The key of the entity.</param>
        public NotFoundException(string name, object key)
            : base($"Entity \"{name}\" ({key}) was not found.")
        {
        }
    }
}