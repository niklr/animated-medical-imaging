using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AMI.Domain.Entities;

namespace AMI.Core.Repositories
{
    /// <summary>
    /// The mock implementation of the Unit of Work pattern.
    /// </summary>
    public class MockUnitOfWork : IAmiUnitOfWork
    {
        /// <inheritdoc/>
        public IRepository<ObjectEntity> ObjectRepository
        {
            get
            {
                return new MockRepository<ObjectEntity>(new List<ObjectEntity>());
            }
        }

        /// <inheritdoc/>
        public IRepository<ResultEntity> ResultRepository
        {
            get
            {
                return new MockRepository<ResultEntity>(new List<ResultEntity>());
            }
        }

        /// <inheritdoc/>
        public IRepository<TaskEntity> TaskRepository
        {
            get
            {
                return new MockRepository<TaskEntity>(new List<TaskEntity>());
            }
        }

        /// <inheritdoc/>
        public bool IsInTransaction => false;

        /// <inheritdoc/>
        public void BeginTransaction()
        {
        }

        /// <inheritdoc/>
        public void CommitTransaction()
        {
        }

        /// <inheritdoc/>
        public void Dispose()
        {
        }

        /// <inheritdoc/>
        public void RollBackTransaction()
        {
        }

        /// <inheritdoc/>
        public int SaveChanges()
        {
            return 0;
        }

        /// <inheritdoc/>
        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await Task.Run(() =>
            {
                return 0;
            });
        }
    }
}
