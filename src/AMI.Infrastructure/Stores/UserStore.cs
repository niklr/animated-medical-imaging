using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Constants;
using AMI.Core.IO.Generators;
using AMI.Core.Repositories;
using AMI.Domain.Entities;
using AMI.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using RNS.Framework.Tools;

namespace AMI.Infrastructure.Stores
{
    /// <summary>
    /// A store which manages user accounts.
    /// </summary>
    /// <typeparam name="TUser">The type encapsulating a user.</typeparam>
    public class UserStore<TUser> : IUserStore<TUser>,
        IUserPasswordStore<TUser>,
        IUserRoleStore<TUser>,
        IQueryableUserStore<TUser>,
        IUserEmailStore<TUser>
        where TUser : UserEntity
    {
        private readonly IAmiUnitOfWork context;
        private readonly IApplicationConstants constants;
        private readonly IIdGenerator idGenerator;
        private readonly IRepository<TUser> repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserStore{TUser}"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="constants">The application constants.</param>
        /// <param name="idGenerator">The generator for unique identifiers.</param>
        public UserStore(IAmiUnitOfWork context, IApplicationConstants constants, IIdGenerator idGenerator)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.constants = constants ?? throw new ArgumentNullException(nameof(constants));
            this.idGenerator = idGenerator ?? throw new ArgumentNullException(nameof(idGenerator));

            repository = context.UserRepository as IRepository<TUser>;
        }

        /// <inheritdoc/>
        public IQueryable<TUser> Users => context.UserRepository.GetQuery() as IQueryable<TUser>;

        /// <inheritdoc/>
        public async Task AddToRoleAsync(TUser user, string roleName, CancellationToken cancellationToken)
        {
            Ensure.ArgumentNotNull(user, nameof(user));

            if (!Enum.TryParse(roleName, true, out RoleType roleType))
            {
                throw new NotSupportedException("The provided role is not supported.");
            }

            var roles = GetRoles(user);
            roles.Add(GenerateInternalRoleName(roleType));
            user.Roles = string.Join(",", roles);

            user.ModifiedDate = DateTime.UtcNow;

            context.UserRepository.Update(user);
            await context.SaveChangesAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<IdentityResult> CreateAsync(TUser user, CancellationToken cancellationToken)
        {
            Ensure.ArgumentNotNull(user, nameof(user));

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
            Ensure.ArgumentNotNull(user, nameof(user));

            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void Dispose()
        {
        }

        /// <inheritdoc/>
        public async Task<TUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            return await repository.GetFirstOrDefaultAsync(e => e.NormalizedEmail == normalizedEmail, cancellationToken);
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
        public Task<string> GetEmailAsync(TUser user, CancellationToken cancellationToken)
        {
            Ensure.ArgumentNotNull(user, nameof(user));

            return Task.FromResult(user.Email);
        }

        /// <inheritdoc/>
        public Task<bool> GetEmailConfirmedAsync(TUser user, CancellationToken cancellationToken)
        {
            Ensure.ArgumentNotNull(user, nameof(user));

            return Task.FromResult(user.EmailConfirmed);
        }

        /// <inheritdoc/>
        public Task<string> GetNormalizedEmailAsync(TUser user, CancellationToken cancellationToken)
        {
            Ensure.ArgumentNotNull(user, nameof(user));

            return Task.FromResult(user.NormalizedEmail);
        }

        /// <inheritdoc/>
        public Task<string> GetNormalizedUserNameAsync(TUser user, CancellationToken cancellationToken)
        {
            Ensure.ArgumentNotNull(user, nameof(user));

            return Task.FromResult(user.NormalizedUsername);
        }

        /// <inheritdoc/>
        public Task<string> GetPasswordHashAsync(TUser user, CancellationToken cancellationToken)
        {
            Ensure.ArgumentNotNull(user, nameof(user));

            return Task.FromResult(user.PasswordHash);
        }

        /// <inheritdoc/>
        public Task<IList<string>> GetRolesAsync(TUser user, CancellationToken cancellationToken)
        {
            Ensure.ArgumentNotNull(user, nameof(user));

            IList<string> roles = string.IsNullOrWhiteSpace(user.Roles) ?
                    Array.Empty<string>() : user.Roles.Replace(constants.RoleNameSeparator, string.Empty).Split(',');

            return Task.FromResult(roles);
        }

        /// <inheritdoc/>
        public Task<string> GetUserIdAsync(TUser user, CancellationToken cancellationToken)
        {
            Ensure.ArgumentNotNull(user, nameof(user));

            return Task.FromResult(user.Id.ToString());
        }

        /// <inheritdoc/>
        public Task<string> GetUserNameAsync(TUser user, CancellationToken cancellationToken)
        {
            Ensure.ArgumentNotNull(user, nameof(user));

            return Task.FromResult(user.Username);
        }

        /// <inheritdoc/>
        public Task<IList<TUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            IList<TUser> result = result = new List<TUser>();

            if (Enum.TryParse(roleName, true, out RoleType roleType))
            {
                var internalRoleName = GenerateInternalRoleName(roleType);
                result = repository.GetQuery(e => !string.IsNullOrWhiteSpace(e.Roles) && e.Roles.Contains(internalRoleName)).ToList();
            }

            return Task.FromResult(result);
        }

        /// <inheritdoc/>
        public Task<bool> HasPasswordAsync(TUser user, CancellationToken cancellationToken)
        {
            Ensure.ArgumentNotNull(user, nameof(user));

            return Task.FromResult(true);
        }

        /// <inheritdoc/>
        public Task<bool> IsInRoleAsync(TUser user, string roleName, CancellationToken cancellationToken)
        {
            if (user == null || !Enum.TryParse(roleName, true, out RoleType roleType))
            {
                return Task.FromResult(false);
            }

            var internalRoleName = GenerateInternalRoleName(roleType);
            var roles = GetRoles(user);

            return Task.FromResult(roles.Contains(internalRoleName));
        }

        /// <inheritdoc/>
        public Task RemoveFromRoleAsync(TUser user, string roleName, CancellationToken cancellationToken)
        {
            Ensure.ArgumentNotNull(user, nameof(user));

            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public async Task SetEmailAsync(TUser user, string email, CancellationToken cancellationToken)
        {
            Ensure.ArgumentNotNull(user, nameof(user));

            user.Email = email;
            await Task.CompletedTask;
        }

        /// <inheritdoc/>
        public async Task SetEmailConfirmedAsync(TUser user, bool confirmed, CancellationToken cancellationToken)
        {
            Ensure.ArgumentNotNull(user, nameof(user));

            user.EmailConfirmed = confirmed;
            await Task.CompletedTask;
        }

        /// <inheritdoc/>
        public async Task SetNormalizedEmailAsync(TUser user, string normalizedEmail, CancellationToken cancellationToken)
        {
            Ensure.ArgumentNotNull(user, nameof(user));

            user.NormalizedEmail = normalizedEmail;
            await Task.CompletedTask;
        }

        /// <inheritdoc/>
        public async Task SetNormalizedUserNameAsync(TUser user, string normalizedName, CancellationToken cancellationToken)
        {
            Ensure.ArgumentNotNull(user, nameof(user));

            user.NormalizedUsername = normalizedName;
            await Task.CompletedTask;
        }

        /// <inheritdoc/>
        public async Task SetPasswordHashAsync(TUser user, string passwordHash, CancellationToken cancellationToken)
        {
            Ensure.ArgumentNotNull(user, nameof(user));

            user.PasswordHash = passwordHash;
            await Task.CompletedTask;
        }

        /// <inheritdoc/>
        public async Task SetUserNameAsync(TUser user, string userName, CancellationToken cancellationToken)
        {
            Ensure.ArgumentNotNull(user, nameof(user));

            user.Username = userName;
            await Task.CompletedTask;
        }

        /// <inheritdoc/>
        public async Task<IdentityResult> UpdateAsync(TUser user, CancellationToken cancellationToken)
        {
            Ensure.ArgumentNotNull(user, nameof(user));

            // TODO: should add an optimistic concurrency check
            user.ModifiedDate = DateTime.UtcNow;
            repository.Update(user);

            await context.SaveChangesAsync(cancellationToken);
            return IdentityResult.Success;
        }

        private string GenerateInternalRoleName(RoleType roleType)
        {
            return $"{constants.RoleNameSeparator}{roleType.ToString()}{constants.RoleNameSeparator}";
        }

        private HashSet<string> GetRoles(TUser user)
        {
            if (user == null || string.IsNullOrWhiteSpace(user.Roles))
            {
                return new HashSet<string>();
            }
            else
            {
                return new HashSet<string>(user.Roles.Split(','));
            }
        }
    }
}
