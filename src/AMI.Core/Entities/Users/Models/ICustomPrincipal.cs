using AMI.Domain.Enums;

namespace AMI.Core.Entities.Models
{
    /// <summary>
    /// An interface representing an abstraction that encapsulates an identity and roles.
    /// </summary>
    public interface ICustomPrincipal
    {
        /// <summary>
        /// Gets the identity.
        /// </summary>
        ICustomIdentity Identity { get; }

        /// <summary>
        /// Determines whether the user is in the specified role.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <returns>
        ///   <c>true</c> if the user is in the specified role; otherwise, <c>false</c>.
        /// </returns>
        bool IsInRole(RoleType role);
    }
}
