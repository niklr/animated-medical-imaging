using System.Collections.Generic;

namespace AMI.Core.Entities.Models
{
    /// <summary>
    /// An interface representing options related to authentication and authorization.
    /// </summary>
    public interface IAuthOptions
    {
        /// <summary>
        /// Gets a value indicating whether anonymous authentication is allowed.
        /// </summary>
        bool AllowAnonymous { get; }

        /// <summary>
        /// Gets the entities allowed to authenticate.
        /// </summary>
        IReadOnlyList<IAuthEntity> Entities { get; }
    }
}
