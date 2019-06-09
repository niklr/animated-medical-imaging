using System;
using AMI.Core.Services;

namespace AMI.Infrastructure.Services
{
    /// <summary>
    /// A service to generate unique identifiers.
    /// </summary>
    /// <seealso cref="IIdGenService" />
    public class IdGenService : IIdGenService
    {
        /// <inheritdoc/>
        public Guid CreateId()
        {
            return Guid.NewGuid();
        }
    }
}
