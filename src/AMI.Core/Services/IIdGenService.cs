using System;

namespace AMI.Core.Services
{
    /// <summary>
    /// An interface representing a service to generate unique identifiers.
    /// </summary>
    public interface IIdGenService
    {
        /// <summary>
        /// Creates a unique identifier.
        /// </summary>
        /// <returns>The unique identifier.</returns>
        Guid CreateId();
    }
}
