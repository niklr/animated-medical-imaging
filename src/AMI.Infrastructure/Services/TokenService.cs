using System;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Configurations;
using AMI.Core.Constants;
using AMI.Core.Entities.Models;
using AMI.Core.Entities.Tokens.Commands.CreateRefreshToken;
using AMI.Core.IO.Serializers;
using AMI.Core.Services;
using AMI.Domain.Entities;
using AMI.Domain.Exceptions;
using AMI.Infrastructure.Wrappers;
using JWT;
using JWT.Algorithms;
using MediatR;
using Microsoft.AspNetCore.Identity;
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
        private readonly IMediator mediator;
        private readonly IJwtEncoder encoder;
        private readonly IJwtDecoder decoder;
        private readonly UserManager<UserEntity> userManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenService"/> class.
        /// </summary>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="configuration">The API configuration.</param>
        /// <param name="mediator">The mediator.</param>
        /// <param name="serializer">The serializer.</param>
        /// <param name="userManager">The user manager.</param>
        public TokenService(
            ILoggerFactory loggerFactory,
            IApiConfiguration configuration,
            IMediator mediator,
            IDefaultJsonSerializer serializer,
            UserManager<UserEntity> userManager)
        {
            logger = loggerFactory?.CreateLogger<ImageService>() ?? throw new ArgumentNullException(nameof(loggerFactory));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            var serializerWrapper = new JwtJsonSerializerWrapper(serializer);
            var urlEncoder = new JwtBase64UrlEncoder();
            encoder = new JwtEncoder(new HMACSHA256Algorithm(), serializerWrapper, urlEncoder);
            var validator = new JwtValidator(serializerWrapper, new UtcDateTimeProvider());
            decoder = new JwtDecoder(serializerWrapper, validator, urlEncoder);
        }

        /// <inheritdoc/>
        public async Task<TokenContainerModel> CreateAsync(string username, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                throw new ArgumentNullException(nameof(username));
            }

            if (ct == null)
            {
                throw new ArgumentNullException(nameof(ct));
            }

            var user = await userManager.FindByNameAsync(username);
            if (user == null)
            {
                throw new UnexpectedNullException("User not found.");
            }

            var result = CreateContainer(UserModel.Create(user));

            var command = new CreateRefreshTokenCommand()
            {
                Token = result.RefreshToken,
                UserId = user.Id.ToString()
            };

            await mediator.Send(command, ct);

            return result;
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
                Username = configuration.Options.AuthOptions.AnonymousUsername,
                Email = $"{configuration.Options.AuthOptions.AnonymousUsername}@localhost".ToLowerInvariant(),
                EmailConfirmed = false
            };

            await Task.CompletedTask;

            return CreateContainer(user);
        }

        /// <inheritdoc/>
        public AccessTokenModel DecodeAccessToken(string token)
        {
            return Decode<AccessTokenModel>(token);
        }

        /// <inheritdoc/>
        public IdTokenModel DecodeIdToken(string token)
        {
            return Decode<IdTokenModel>(token);
        }

        /// <inheritdoc/>
        public RefreshTokenModel DecodeRefreshToken(string token)
        {
            return Decode<RefreshTokenModel>(token);
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

        private T Decode<T>(string token)
        {
            return decoder.DecodeToObject<T>(token, configuration.Options.AuthOptions.JwtOptions.SecretKey, true);
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
            token.IsAnon = user.Username == configuration.Options.AuthOptions.AnonymousUsername;
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
