using System;
using AMI.Core.IO.Generators;

namespace AMI.Infrastructure.IO.Generators
{
    /// <summary>
    /// A generator for unique identifiers.
    /// </summary>
    /// <seealso cref="IIdGenerator" />
    public class IdGenerator : IIdGenerator
    {
        /// <inheritdoc/>
        public Guid GenerateId()
        {
            return Guid.NewGuid();
        }
    }
}
