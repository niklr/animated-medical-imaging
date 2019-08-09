using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Configurations;
using AMI.Core.Entities.Models;
using AMI.Core.Entities.Shared.Commands;
using AMI.Core.IO.Generators;
using AMI.Core.Modules;
using AMI.Domain.Entities;
using AMI.Domain.Exceptions;
using RNS.Framework.Extensions.MutexExtensions;
using RNS.Framework.Extensions.Reflection;

namespace AMI.Core.Entities.Tokens.Commands.CreateRefreshToken
{
    /// <summary>
    /// A handler for requests to create refresh tokens.
    /// </summary>
    public class CreateCommandHandler : BaseCommandRequestHandler<CreateRefreshTokenCommand, TokenModel>
    {
        private static Mutex processMutex;

        private readonly IIdGenerator idGenerator;
        private readonly IApiConfiguration configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateCommandHandler"/> class.
        /// </summary>
        /// <param name="module">The command handler module.</param>
        /// <param name="idGenerator">The generator for unique identifiers.</param>
        /// <param name="configuration">The API configuration.</param>
        public CreateCommandHandler(
            ICommandHandlerModule module,
            IIdGenerator idGenerator,
            IApiConfiguration configuration)
            : base(module)
        {
            this.idGenerator = idGenerator ?? throw new ArgumentNullException(nameof(idGenerator));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        /// <inheritdoc/>
        protected override async Task<TokenModel> ProtectedHandleAsync(CreateRefreshTokenCommand request, CancellationToken cancellationToken)
        {
            processMutex = new Mutex(false, this.GetMethodName());

            return await processMutex.Execute(new TimeSpan(0, 0, 2), async () =>
            {
                var userId = Guid.Parse(request.UserId);

                var user = await Context.UserRepository.GetFirstOrDefaultAsync(e => e.Id == userId, cancellationToken);
                if (user == null)
                {
                    throw new UnexpectedNullException("User not found.");
                }

                // Make sure the amount of valid refresh tokens for a single user don't exceed the limit
                var tokens = Context.TokenRepository.GetQuery().Where(e => e.UserId == userId);
                if (tokens.Count() >= configuration.Options.AuthOptions.MaxRefreshTokens)
                {
                    var unusedToken = tokens.OrderBy(e => e.LastUsedDate).FirstOrDefault();
                    Context.TokenRepository.Remove(unusedToken);
                }

                var token = new TokenEntity()
                {
                    Id = idGenerator.GenerateId(),
                    CreatedDate = DateTime.UtcNow,
                    LastUsedDate = DateTime.UtcNow,
                    UserId = Guid.Parse(request.UserId),
                    TokenValue = request.Token
                };

                Context.TokenRepository.Add(token);

                await Context.SaveChangesAsync(cancellationToken);

                return TokenModel.Create(token);
            });
        }
    }
}
