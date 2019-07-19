using System.Threading.Tasks;
using AMI.Core.Entities.Models;

namespace AMI.Core.Services
{
    /// <summary>
    /// An interface representing a service for tokens related to authentication and authorization.
    /// </summary>
    public interface ITokenService
    {
        /// <summary>
        /// Creates a token container asynchronous.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns>The token container.</returns>
        Task<TokenContainerModel> CreateAsync(string username);

        /// <summary>
        /// Creates a token container for an anonymous user asynchronous.
        /// </summary>
        /// <returns>The token container.</returns>
        Task<TokenContainerModel> CreateAnonymousAsync();

        /// <summary>
        /// Uses the refresh token to get a new token container asynchronous.
        /// </summary>
        /// <param name="token">The value of the refresh token.</param>
        /// <returns>The new token container.</returns>
        Task<TokenContainerModel> UseRefreshTokenAsync(string token);
    }
}
