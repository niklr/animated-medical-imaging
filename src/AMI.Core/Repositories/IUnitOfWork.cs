using System;

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
        /// Saves the changes to the underlying repositories.
        /// </summary>
        void SaveChanges();

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
