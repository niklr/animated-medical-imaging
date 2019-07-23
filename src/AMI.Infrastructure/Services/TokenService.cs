using System;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Configurations;
using AMI.Core.Entities.Models;
using AMI.Core.Entities.Tokens.Commands.CreateRefreshToken;
using AMI.Core.Entities.Tokens.Commands.UpdateRefreshToken;
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
        public async Task<TokenContainerModel> CreateAsync(string username, string password, CancellationToken ct)
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
                throw new UnexpectedNullException("Incorrect username or password.");
            }

            bool isValid = await userManager.CheckPasswordAsync(user, password);

            if (!isValid)
            {
                throw new UnexpectedNullException("Incorrect username or password.");
            }

            var result = await CreateContainerAsync(UserModel.Create(user), ct);

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

            var user = CreateAnonymousUserModel(Guid.NewGuid().ToString());
            return await CreateContainerAsync(user, ct);
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
                var user = CreateAnonymousUserModel(decoded.Sub);
                var result = await CreateContainerAsync(user, ct);
                result.RefreshToken = token;

                return result;
            }
            else
            {
                var user = await userManager.FindByIdAsync(decoded.Sub);
                if (user == null)
                {
                    throw new UnexpectedNullException("User not found.");
                }

                var command = new UpdateRefreshTokenCommand()
                {
                    Token = token,
                    UserId = user.Id.ToString()
                };

                await mediator.Send(command, ct);

                var result = await CreateContainerAsync(UserModel.Create(user), ct);
                result.RefreshToken = token;

                return result;
            }
        }

        private UserModel CreateAnonymousUserModel(string userId)
        {
            if (!configuration.Options.AuthOptions.AllowAnonymous)
            {
                throw new AmiException("Anonymous users are not allowed.");
            }

            return new UserModel()
            {
                Id = userId,
                Username = configuration.Options.AuthOptions.AnonymousUsername,
                Email = $"{configuration.Options.AuthOptions.AnonymousUsername}@localhost".ToLowerInvariant(),
                EmailConfirmed = false
            };
        }

        private T Decode<T>(string token)
        {
            try
            {
                return decoder.DecodeToObject<T>(token, configuration.Options.AuthOptions.JwtOptions.SecretKey, true);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                throw new AmiException("The provided token could not be decoded.");
            }
        }

        private async Task<TokenContainerModel> CreateContainerAsync(UserModel user, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            var secretKey = configuration.Options.AuthOptions.JwtOptions.SecretKey;
            var model = new TokenContainerModel()
            {
                AccessToken = encoder.Encode(CreateAccessToken(user), secretKey),
                IdToken = encoder.Encode(CreateIdToken(user), secretKey),
                RefreshToken = encoder.Encode(CreateRefreshToken(user), secretKey)
            };

            await Task.CompletedTask;

            return model;
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
