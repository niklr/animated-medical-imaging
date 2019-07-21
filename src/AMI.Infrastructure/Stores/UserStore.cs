using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.IO.Generators;
using AMI.Core.Repositories;
using AMI.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace AMI.Infrastructure.Stores
{
    /// <summary>
    /// A store which manages user accounts.
    /// </summary>
    /// <typeparam name="TUser">The type encapsulating a user.</typeparam>
    public class UserStore<TUser> : IUserStore<TUser>,
        IUserPasswordStore<TUser>,
        IQueryableUserStore<TUser>
        where TUser : UserEntity
    {
        private readonly IAmiUnitOfWork context;
        private readonly IIdGenerator idGenerator;
        private readonly IRepository<TUser> repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserStore{TUser}"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="idGenerator">The generator for unique identifiers.</param>
        public UserStore(IAmiUnitOfWork context, IIdGenerator idGenerator)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.idGenerator = idGenerator ?? throw new ArgumentNullException(nameof(idGenerator));

            repository = context.UserRepository as IRepository<TUser>;
        }

        /// <inheritdoc/>
        public IQueryable<TUser> Users => context.UserRepository.GetQuery() as IQueryable<TUser>;

        /// <inheritdoc/>
        public async Task<IdentityResult> CreateAsync(TUser user, CancellationToken cancellationToken)
        {
            user.Id = idGenerator.GenerateId();
            user.CreatedDate = DateTime.UtcNow;
            user.ModifiedDate = DateTime.UtcNow;
            context.UserRepository.Add(user);
            await context.SaveChangesAsync(cancellationToken);
            return IdentityResult.Success;
        }

        /// <inheritdoc/>
        public Task<IdentityResult> DeleteAsync(TUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void Dispose()
        {
        }

        /// <inheritdoc/>
        public async Task<TUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            if (Guid.TryParse(userId, out Guid guid))
            {
                return await repository.GetFirstOrDefaultAsync(e => e.Id == guid, cancellationToken);
            }
            else
            {
                await Task.CompletedTask;
                return null;
            }
        }

        /// <inheritdoc/>
        public async Task<TUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            return await repository.GetFirstOrDefaultAsync(e => e.NormalizedUsername == normalizedUserName, cancellationToken);
        }

        /// <inheritdoc/>
        public Task<string> GetNormalizedUserNameAsync(TUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.NormalizedUsername);
        }

        /// <inheritdoc/>
        public Task<string> GetPasswordHashAsync(TUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash);
        }

        /// <inheritdoc/>
        public Task<string> GetUserIdAsync(TUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Id.ToString());
        }

        /// <inheritdoc/>
        public Task<string> GetUserNameAsync(TUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Username);
        }

        /// <inheritdoc/>
        public Task<bool> HasPasswordAsync(TUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(true);
        }

        /// <inheritdoc/>
        public async Task SetNormalizedUserNameAsync(TUser user, string normalizedName, CancellationToken cancellationToken)
        {
            user.NormalizedUsername = normalizedName;
            await Task.CompletedTask;
        }

        /// <inheritdoc/>
        public async Task SetPasswordHashAsync(TUser user, string passwordHash, CancellationToken cancellationToken)
        {
            user.PasswordHash = passwordHash;
            await Task.CompletedTask;
        }

        /// <inheritdoc/>
        public async Task SetUserNameAsync(TUser user, string userName, CancellationToken cancellationToken)
        {
            user.Username = userName;
            await Task.CompletedTask;
        }

        /// <inheritdoc/>
        public async Task<IdentityResult> UpdateAsync(TUser user, CancellationToken cancellationToken)
        {
            // TODO: should add an optimistic concurrency check
            user.ModifiedDate = DateTime.UtcNow;
            repository.Update(user);
            await context.SaveChangesAsync(cancellationToken);
            return IdentityResult.Success;
        }
    }
}
