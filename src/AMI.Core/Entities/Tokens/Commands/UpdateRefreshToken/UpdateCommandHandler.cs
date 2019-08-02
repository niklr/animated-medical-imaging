using System;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Entities.Models;
using AMI.Core.Entities.Shared.Commands;
using AMI.Core.Modules;
using AMI.Domain.Exceptions;

namespace AMI.Core.Entities.Tokens.Commands.UpdateRefreshToken
{
    /// <summary>
    /// A handler for requests to update refresh tokens.
    /// </summary>
    public class UpdateCommandHandler : BaseCommandRequestHandler<UpdateRefreshTokenCommand, TokenModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateCommandHandler"/> class.
        /// </summary>
        /// <param name="module">The command handler module.</param>
        public UpdateCommandHandler(ICommandHandlerModule module)
            : base(module)
        {
        }

        /// <inheritdoc/>
        protected override async Task<TokenModel> ProtectedHandleAsync(UpdateRefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var userId = Guid.Parse(request.UserId);
            var token = await Context.TokenRepository.GetFirstOrDefaultAsync(
                e => e.TokenValue == request.Token && e.UserId == userId, cancellationToken);

            if (token == null)
            {
                throw new UnexpectedNullException("Refresh token not found.");
            }

            token.LastUsedDate = DateTime.UtcNow;

            Context.TokenRepository.Update(token);

            await Context.SaveChangesAsync(cancellationToken);

            return TokenModel.Create(token);
        }
    }
}
