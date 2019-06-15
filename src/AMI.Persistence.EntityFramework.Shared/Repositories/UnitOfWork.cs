using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
        public IQueryable<T> Include<T>(IQueryable<T> source, Expression<Func<T, bool>> navigationPropertyPath)
            where T : class
        {
            return source.Include(navigationPropertyPath);
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

        /// <inheritdoc/>
        public async Task<List<T>> ToListAsync<T>(IQueryable<T> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
            {
                return null;
            }

            return await source.ToListAsync(cancellationToken);
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
