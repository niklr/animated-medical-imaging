namespace AMI.Core.Services
{
    /// <summary>
    /// An interface representing a service for authorization purposes.
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Determines whether the authenticated user is authorized.
        /// </summary>
        /// <param name="ownerId">The identifier of the owner.</param>
        /// <returns>
        ///   <c>true</c> if the authenticated user is authorized; otherwise, <c>false</c>.
        /// </returns>
        bool IsAuthorized(string ownerId);
    }
}
