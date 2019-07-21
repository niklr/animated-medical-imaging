using System;

namespace AMI.Core.IO.Generators
{
    /// <summary>
    /// An interface representing a generator for unique identifiers.
    /// </summary>
    public interface IIdGenerator
    {
        /// <summary>
        /// Generates a unique identifier.
        /// </summary>
        /// <returns>The unique identifier.</returns>
        Guid GenerateId();
    }
}
