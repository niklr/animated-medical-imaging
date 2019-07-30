using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Configurations;
using AMI.Core.Constants;
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
using RNS.Framework.Tools;

namespace AMI.Infrastructure.Services
{
    /// <summary>
    /// A service for tokens related to authentication and authorization.
    /// </summary>
    public class TokenService : ITokenService
    {
        private readonly ILogger logger;
        private readonly IApiConfiguration configuration;
        private readonly IApplicationConstants constants;
        private readonly IMediator mediator;
        private readonly IDefaultJsonSerializer serializer;
        private readonly IJwtEncoder encoder;
        private readonly IJwtDecoder decoder;
        private readonly UserManager<UserEntity> userManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenService"/> class.
        /// </summary>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="configuration">The API configuration.</param>
        /// <param name="constants">The application constants.</param>
        /// <param name="mediator">The mediator.</param>
        /// <param name="serializer">The serializer.</param>
        /// <param name="userManager">The user manager.</param>
        public TokenService(
            ILoggerFactory loggerFactory,
            IApiConfiguration configuration,
            IApplicationConstants constants,
            IMediator mediator,
            IDefaultJsonSerializer serializer,
            UserManager<UserEntity> userManager)
        {
            logger = loggerFactory?.CreateLogger<ImageService>() ?? throw new ArgumentNullException(nameof(loggerFactory));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.constants = constants ?? throw new ArgumentNullException(nameof(constants));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            this.serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
            var serializerWrapper = new JwtJsonSerializerWrapper(serializer);
            var urlEncoder = new JwtBase64UrlEncoder();
            encoder = new JwtEncoder(new HMACSHA256Algorithm(), serializerWrapper, urlEncoder);
            var validator = new JwtValidator(serializerWrapper, new UtcDateTimeProvider());
            decoder = new JwtDecoder(serializerWrapper, validator, urlEncoder);
        }

        /// <inheritdoc/>
        public async Task<TokenContainerModel> CreateAsync(string username, string password, CancellationToken ct)
        {
            Ensure.ArgumentNotNull(username, nameof(username));
            Ensure.ArgumentNotNull(password, nameof(password));
            Ensure.ArgumentNotNull(ct, nameof(ct));

            ct.ThrowIfCancellationRequested();

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

            var result = await CreateContainerAsync(UserModel.Create(user, constants), ct);

            var command = new CreateRefreshTokenCommand()
            {
                Token = result.RefreshTokenEncoded,
                UserId = user.Id.ToString()
            };

            await mediator.Send(command, ct);

            return result;
        }

        /// <inheritdoc/>
        public async Task<TokenContainerModel> CreateAnonymousAsync(CancellationToken ct)
        {
            Ensure.ArgumentNotNull(ct, nameof(ct));

            ct.ThrowIfCancellationRequested();

            var user = CreateAnonymousUserModel(Guid.NewGuid().ToString());
            return await CreateContainerAsync(user, ct);
        }

        /// <inheritdoc/>
        public async Task<BaseTokenModel> DecodeAsync(string token, CancellationToken ct)
        {
            Ensure.ArgumentNotNull(ct, nameof(ct));

            ct.ThrowIfCancellationRequested();

            try
            {
                var decoded = decoder.Decode(token, configuration.Options.AuthOptions.JwtOptions.SecretKey, true);
                var result = serializer.Deserialize<BaseTokenModel>(decoded);

                await Task.CompletedTask;

                return result;
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                throw new AmiException("The provided token could not be decoded.");
            }
        }

        /// <inheritdoc/>
        public async Task<TokenContainerModel> UseRefreshTokenAsync(string token, CancellationToken ct)
        {
            Ensure.ArgumentNotNull(ct, nameof(ct));

            ct.ThrowIfCancellationRequested();

            var decoded = await DecodeAsync(token, ct);
            if (decoded == null || !(decoded is RefreshTokenModel))
            {
                throw new UnexpectedNullException("The provided refresh token could not be decoded.");
            }

            if (decoded.IsAnon)
            {
                var user = CreateAnonymousUserModel(decoded.Sub);
                var result = await CreateContainerAsync(user, ct);
                result.RefreshTokenEncoded = token;

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

                var result = await CreateContainerAsync(UserModel.Create(user, constants), ct);
                result.RefreshTokenEncoded = token;

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

        private async Task<TokenContainerModel> CreateContainerAsync(UserModel user, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            var secretKey = Encoding.ASCII.GetBytes(configuration.Options.AuthOptions.JwtOptions.SecretKey);
            var accessToken = CreateAccessToken(user);
            var idToken = CreateIdToken(user);
            var refreshToken = CreateRefreshToken(user);
            var model = new TokenContainerModel()
            {
                AccessToken = accessToken,
                AccessTokenEncoded = encoder.Encode(accessToken, secretKey),
                IdToken = idToken,
                IdTokenEncoded = encoder.Encode(idToken, secretKey),
                RefreshToken = refreshToken,
                RefreshTokenEncoded = encoder.Encode(refreshToken, secretKey)
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
                Exp = DateTime.Now.AddMinutes(configuration.Options.AuthOptions.ExpireAfter).ToUnix(),
                Username = user.Username,
                RoleClaims = user.Roles
            };

            SetBaseToken(token, user);

            return token;
        }

        private IdTokenModel CreateIdToken(UserModel user)
        {
            var token = new IdTokenModel()
            {
                Email = user.Email,
                EmailConfirmed = user.EmailConfirmed
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
