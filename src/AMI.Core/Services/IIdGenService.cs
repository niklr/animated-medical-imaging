using System;

namespace AMI.Core.Services
{
    /// <summary>
    /// A service to generate unique identifiers.
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
