using System;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace AMI.Persistence.EntityFramework.Shared.Repositories
{
    /// <summary>
    /// An implementation of the Unit of Work pattern.
    /// </summary>
    /// <seealso cref="IUnitOfWork" />
    public abstract class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext context;
        private IDbContextTransaction transaction;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWork"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        public UnitOfWork(DbContext context)
        {
            this.context = context;
        }

        /// <inheritdoc/>
        public bool IsInTransaction
        {
            get { return transaction != null; }
        }

        /// <inheritdoc/>
        public void BeginTransaction()
        {
            if (transaction == null)
            {
                transaction = context.Database.BeginTransaction();
            }
        }

        /// <inheritdoc/>
        public void CommitTransaction()
        {
            if (transaction == null)
            {
                throw new ApplicationException("Cannot roll back a transaction while there is no transaction running.");
            }

            try
            {
                SaveChanges();
                transaction.Commit();
                ReleaseCurrentTransaction();
            }
            catch
            {
                RollBackTransaction();
                throw;
            }
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        /// <inheritdoc/>
        public void RollBackTransaction()
        {
            if (transaction == null)
            {
                throw new ApplicationException("Cannot roll back a transaction while there is no transaction running.");
            }

            if (IsInTransaction)
            {
                transaction.Rollback();
                ReleaseCurrentTransaction();
            }
        }

        /// <inheritdoc/>
        public int SaveChanges()
        {
            return context.SaveChanges();
        }

        /// <inheritdoc/>
        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return context.SaveChangesAsync(cancellationToken);
        }

        private void ReleaseCurrentTransaction()
        {
            if (transaction != null)
            {
                transaction.Dispose();
                transaction = null;
            }
        }
    }
}
