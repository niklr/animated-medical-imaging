using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Repositories;
using AMI.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace AMI.Infrastructure.Stores
{
    /// <summary>
    /// A store to manage roles.
    /// </summary>
    /// <typeparam name="TRole">The type that represents a role.</typeparam>
    public class RoleStore<TRole> : IRoleStore<TRole>,
        IQueryableRoleStore<TRole>
        where TRole : RoleEntity
    {
        private readonly IAmiUnitOfWork context;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleStore{TRole}"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public RoleStore(IAmiUnitOfWork context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <inheritdoc/>
        public IQueryable<TRole> Roles => throw new NotImplementedException();

        /// <inheritdoc/>
        public Task<IdentityResult> CreateAsync(TRole role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<IdentityResult> DeleteAsync(TRole role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<TRole> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<TRole> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<string> GetNormalizedRoleNameAsync(TRole role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<string> GetRoleIdAsync(TRole role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<string> GetRoleNameAsync(TRole role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task SetNormalizedRoleNameAsync(TRole role, string normalizedName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task SetRoleNameAsync(TRole role, string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<IdentityResult> UpdateAsync(TRole role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
