using System;
using System.Threading;
using System.Threading.Tasks;

namespace AMI.Core.Repositories
{
    /// <summary>
    /// Perform a set of operations in one unit against repositories.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Gets a value indicating whether this instance is in a transaction.
        /// </summary>
        bool IsInTransaction { get; }

        /// <summary>
        /// Saves all changes made in this context to the database.
        /// </summary>
        /// <returns>The number of state entries written to the database.</returns>
        int SaveChanges();

        /// <summary>
        /// Asynchronously saves all changes made in this context to the database.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task that represents the asynchronous save operation.
        /// The task result contains the number of state entries written to the database.</returns>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Begins a new transaction.
        /// </summary>
        void BeginTransaction();

        /// <summary>
        /// Rolls the transaction back.
        /// </summary>
        void RollBackTransaction();

        /// <summary>
        /// Commits the transaction.
        /// </summary>
        void CommitTransaction();
    }
}
