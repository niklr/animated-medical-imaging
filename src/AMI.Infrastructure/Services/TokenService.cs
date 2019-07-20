using System;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Configurations;
using AMI.Core.Constants;
using AMI.Core.Entities.Models;
using AMI.Core.IO.Serializers;
using AMI.Core.Services;
using AMI.Domain.Exceptions;
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
        private readonly IApplicationConstants constants;
        private readonly IApiConfiguration configuration;
        private readonly IJwtEncoder encoder;
        private readonly IJwtDecoder decoder;

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenService"/> class.
        /// </summary>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="constants">The application constants.</param>
        /// <param name="configuration">The API configuration.</param>
        /// <param name="serializer">The serializer.</param>
        public TokenService(
            ILoggerFactory loggerFactory,
            IApplicationConstants constants,
            IApiConfiguration configuration,
            IDefaultJsonSerializer serializer)
        {
            logger = loggerFactory?.CreateLogger<ImageService>() ?? throw new ArgumentNullException(nameof(loggerFactory));
            this.constants = constants ?? throw new ArgumentNullException(nameof(constants));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            var serializerWrapper = new JwtJsonSerializerWrapper(serializer);
            var urlEncoder = new JwtBase64UrlEncoder();
            encoder = new JwtEncoder(new HMACSHA256Algorithm(), serializerWrapper, urlEncoder);
            var validator = new JwtValidator(serializerWrapper, new UtcDateTimeProvider());
            decoder = new JwtDecoder(serializerWrapper, validator, urlEncoder);
        }

        /// <inheritdoc/>
        public Task<TokenContainerModel> CreateAsync(string username, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public async Task<TokenContainerModel> CreateAnonymousAsync(CancellationToken ct)
        {
            if (ct == null)
            {
                throw new ArgumentNullException(nameof(ct));
            }

            ct.ThrowIfCancellationRequested();

            if (!configuration.Options.AuthOptions.AllowAnonymous)
            {
                throw new AmiException("Anonymous users are not allowed.");
            }

            var user = new UserModel()
            {
                Id = Guid.NewGuid().ToString(),
                Username = constants.AnonymousUsername,
                Email = $"{constants.AnonymousUsername}@localhost".ToLowerInvariant(),
                EmailConfirmed = false
            };

            await Task.CompletedTask;

            return CreateContainer(user);
        }

        /// <inheritdoc/>
        public AccessTokenModel DecodeAccessToken(string token)
        {
            return decoder.DecodeToObject<AccessTokenModel>(token);
        }

        /// <inheritdoc/>
        public IdTokenModel DecodeIdToken(string token)
        {
            return decoder.DecodeToObject<IdTokenModel>(token);
        }

        /// <inheritdoc/>
        public RefreshTokenModel DecodeRefreshToken(string token)
        {
            return decoder.DecodeToObject<RefreshTokenModel>(token);
        }

        /// <inheritdoc/>
        public async Task<TokenContainerModel> UseRefreshTokenAsync(string token, CancellationToken ct)
        {
            if (ct == null)
            {
                throw new ArgumentNullException(nameof(ct));
            }

            ct.ThrowIfCancellationRequested();

            var decoded = DecodeRefreshToken(token);
            if (decoded == null)
            {
                throw new UnexpectedNullException("The provided refresh token could not be decoded.");
            }

            if (decoded.IsAnon)
            {
                return await CreateAnonymousAsync(ct);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private TokenContainerModel CreateContainer(UserModel user)
        {
            var secretKey = configuration.Options.AuthOptions.JwtOptions.SecretKey;
            return new TokenContainerModel()
            {
                AccessToken = encoder.Encode(CreateAccessToken(user), secretKey),
                IdToken = encoder.Encode(CreateIdToken(user), secretKey),
                RefreshToken = encoder.Encode(CreateRefreshToken(user), secretKey)
            };
        }

        private void SetBaseToken(BaseTokenModel token, UserModel user)
        {
            int now = DateTime.Now.ToUnix();
            token.Sub = user.Id;
            token.Iss = configuration.Options.AuthOptions.JwtOptions.Issuer;
            token.Aud = configuration.Options.AuthOptions.JwtOptions.Audience;
            token.Nbf = now;
            token.Iat = now;
            token.IsAnon = user.Username == constants.AnonymousUsername;
        }

        private AccessTokenModel CreateAccessToken(UserModel user)
        {
            var token = new AccessTokenModel()
            {
                Exp = DateTime.Now.AddMinutes(configuration.Options.AuthOptions.ExpireAfter).ToUnix()
            };

            SetBaseToken(token, user);

            return token;
        }

        private IdTokenModel CreateIdToken(UserModel user)
        {
            var token = new IdTokenModel()
            {
                Email = user.Email,
                EmailConfirmed = user.EmailConfirmed,
                Username = user.Username,
                Roles = user.Roles
            };

            SetBaseToken(token, user);

            return token;
        }

        private RefreshTokenModel CreateRefreshToken(UserModel user)
        {
            var token = new RefreshTokenModel();

            SetBaseToken(token, user);

            return token;
        }
    }
}
