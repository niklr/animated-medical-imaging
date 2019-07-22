using AMI.Core.Entities.Models;
using MediatR;

namespace AMI.Core.Entities.Tokens.Commands.CreateRefreshToken
{
    /// <summary>
    /// A command containing information needed to create a refresh token.
    /// </summary>
    public class CreateRefreshTokenCommand : IRequest<TokenModel>
    {
        /// <summary>
        /// Gets or sets the identifier of the user.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the value of the token.
        /// </summary>
        public string Token { get; set; }
    }
}
