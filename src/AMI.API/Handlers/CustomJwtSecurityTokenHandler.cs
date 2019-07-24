using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace AMI.API.Handlers
{
    /// <summary>
    /// A custom implementation to handle JWT security tokens.
    /// </summary>
    /// <seealso cref="ISecurityTokenValidator" />
    public class CustomJwtSecurityTokenHandler : ISecurityTokenValidator
    {
        private JwtSecurityTokenHandler tokenHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomJwtSecurityTokenHandler" /> class.
        /// </summary>
        public CustomJwtSecurityTokenHandler()
        {
            tokenHandler = new JwtSecurityTokenHandler();
        }

        /// <inheritdoc/>
        public bool CanValidateToken
        {
            get
            {
                return true;
            }
        }

        /// <inheritdoc/>
        public int MaximumTokenSizeInBytes { get; set; } = TokenValidationParameters.DefaultMaximumTokenSizeInBytes;

        /// <inheritdoc/>
        public bool CanReadToken(string securityToken)
        {
            return tokenHandler.CanReadToken(securityToken);
        }

        /// <inheritdoc/>
        public ClaimsPrincipal ValidateToken(string securityToken, TokenValidationParameters validationParameters, out SecurityToken validatedToken)
        {
            return tokenHandler.ValidateToken(securityToken, validationParameters, out validatedToken);
        }
    }
}
