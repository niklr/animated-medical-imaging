using System.Threading;
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
        /// <param name="password">The password.</param>
        /// <param name="ct">The cancellation token.</param>
        /// <returns>The token container.</returns>
        Task<TokenContainerModel> CreateAsync(string username, string password, CancellationToken ct);

        /// <summary>
        /// Creates a token container for an anonymous user asynchronous.
        /// </summary>
        /// <param name="ct">The cancellation token.</param>
        /// <returns>The token container.</returns>
        Task<TokenContainerModel> CreateAnonymousAsync(CancellationToken ct);

        /// <summary>
        /// Decodes the specified access token.
        /// </summary>
        /// <param name="token">The value of the access token.</param>
        /// <returns>The decoded access token.</returns>
        AccessTokenModel DecodeAccessToken(string token);

        /// <summary>
        /// Decodes the specified identity token.
        /// </summary>
        /// <param name="token">The value of the identity token.</param>
        /// <returns>The decoded identity token.</returns>
        IdTokenModel DecodeIdToken(string token);

        /// <summary>
        /// Decodes the specified refresh token.
        /// </summary>
        /// <param name="token">The value of the refresh token.</param>
        /// <returns>The decoded refresh token.</returns>
        RefreshTokenModel DecodeRefreshToken(string token);

        /// <summary>
        /// Uses the refresh token to get a new token container asynchronous.
        /// </summary>
        /// <param name="token">The value of the refresh token.</param>
        /// <param name="ct">The cancellation token.</param>
        /// <returns>The new token container.</returns>
        Task<TokenContainerModel> UseRefreshTokenAsync(string token, CancellationToken ct);
    }
}
