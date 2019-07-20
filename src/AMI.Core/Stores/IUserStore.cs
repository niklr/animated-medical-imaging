using System;
using System.Threading;
using System.Threading.Tasks;

namespace AMI.Core.Stores
{
    /// <summary>
    /// Provides an abstraction for a store which manages user accounts.
    /// </summary>
    /// <typeparam name="TUser">The type encapsulating a user.</typeparam>
    public interface IUserStore<TUser> : IDisposable
        where TUser : class
    {
        /// <summary>
        /// Creates the specified user in the user store.
        /// </summary>
        /// <param name="user">The user to create.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, 
        /// containing the <see cref="bool"/> of the creation operation.
        /// </returns>
        Task<bool> CreateAsync(TUser user, CancellationToken cancellationToken);
    }
}
