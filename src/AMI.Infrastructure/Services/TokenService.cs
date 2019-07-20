using System;
using System.Threading.Tasks;
using AMI.Core.Configurations;
using AMI.Core.Entities.Models;
using AMI.Core.IO.Serializers;
using AMI.Core.Services;
using AMI.Infrastructure.Wrappers;
using JWT;
using JWT.Algorithms;
using Microsoft.Extensions.Logging;
using RNS.Framework.Extensions.Date;

namespace AMI.Infrastructure.Services
{
    /// <summary>
    /// A service for tokens related to authentication and authorization.
    /// </summary>
    public class TokenService : ITokenService
    {
        private readonly ILogger logger;
        private readonly IApiConfiguration configuration;
        private readonly IJwtEncoder encoder;
        private readonly IJwtDecoder decoder;

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenService"/> class.
        /// </summary>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="serializer">The serializer.</param>
        public TokenService(
            ILoggerFactory loggerFactory,
            IApiConfiguration configuration,
            IDefaultJsonSerializer serializer)
        {
            logger = loggerFactory?.CreateLogger<ImageService>() ?? throw new ArgumentNullException(nameof(loggerFactory));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            var serializerWrapper = new JwtJsonSerializerWrapper(serializer);
            var urlEncoder = new JwtBase64UrlEncoder();
            encoder = new JwtEncoder(new HMACSHA256Algorithm(), serializerWrapper, urlEncoder);
            var validator = new JwtValidator(serializerWrapper, new UtcDateTimeProvider());
            decoder = new JwtDecoder(serializerWrapper, validator, urlEncoder);
        }

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

        private void SetBaseToken(BaseTokenModel token)
        {
            int now = DateTime.Now.ToUnix();
            token.Iss = configuration.Options.AuthOptions.JwtOptions.Issuer;
            token.Aud = configuration.Options.AuthOptions.JwtOptions.Audience;
            token.Nbf = now;
            token.Iat = now;
        }

        private AccessTokenModel CreateAccessToken(string id)
        {
            var token = new AccessTokenModel()
            {
                Sub = id,
                Exp = DateTime.Now.AddMinutes(configuration.Options.AuthOptions.ExpireAfter).ToUnix()
            };

            SetBaseToken(token);

            return token;
        }

        private IdTokenModel CreateIdToken(UserModel user)
        {
            var token = new IdTokenModel()
            {
                Sub = user.Id,
                Email = user.Email,
                EmailConfirmed = user.EmailConfirmed,
                Username = user.Username,
                Roles = user.Roles
            };

            SetBaseToken(token);

            return token;
        }

        private RefreshTokenModel CreateRefreshToken(string id)
        {
            var token = new RefreshTokenModel()
            {
                Sub = id
            };

            SetBaseToken(token);

            return token;
        }
    }
}
