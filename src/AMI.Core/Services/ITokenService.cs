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
        /// Decodes the specified token.
        /// </summary>
        /// <param name="token">The value of the token.</param>
        /// <param name="ct">The cancellation token.</param>
        /// <typeparam name="T">The type of the token.</typeparam>
        /// <returns>The decoded token.</returns>
        Task<T> DecodeAsync<T>(string token, CancellationToken ct)
            where T : BaseTokenModel;

        /// <summary>
        /// Determines whether the specified token represents an anonymous user.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns>
        ///   <c>true</c> if the specified token represents an anonymous user; otherwise, <c>false</c>.
        /// </returns>
        bool IsAnonymous(BaseTokenModel token);

        /// <summary>
        /// Uses the refresh token to get a new token container asynchronous.
        /// </summary>
        /// <param name="token">The value of the refresh token.</param>
        /// <param name="ct">The cancellation token.</param>
        /// <returns>The new token container.</returns>
        Task<TokenContainerModel> UseRefreshTokenAsync(string token, CancellationToken ct);
    }
}
