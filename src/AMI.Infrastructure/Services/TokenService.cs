using System;
using System.Threading.Tasks;
using AMI.Core.Entities.Models;
using AMI.Core.Services;

namespace AMI.Infrastructure.Services
{
    /// <summary>
    /// A service for tokens related to authentication and authorization.
    /// </summary>
    public class TokenService : ITokenService
    {
        /// <inheritdoc/>
        public Task<TokenContainerModel> CreateAsync(string username)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<TokenContainerModel> CreateAnonymousAsync()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<TokenContainerModel> UseRefreshTokenAsync(string token)
        {
            throw new NotImplementedException();
        }
    }
}
