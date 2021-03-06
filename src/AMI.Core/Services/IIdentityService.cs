﻿using System.Threading;
using System.Threading.Tasks;

namespace AMI.Core.Services
{
    /// <summary>
    /// An interface representing a service for identities related to security.
    /// </summary>
    public interface IIdentityService
    {
        /// <summary>
        /// Ensures that the users defined in the application configuration exist.
        /// </summary>
        /// <param name="ct">The cancellation token.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task EnsureUsersExistAsync(CancellationToken ct);
    }
}
