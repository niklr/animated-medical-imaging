﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
        public IQueryable<T> Include<T>(IQueryable<T> source, Expression<Func<T, bool>> navigationPropertyPath)
            where T : class
        {
            return source;
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

        /// <inheritdoc/>
        public async Task<List<T>> ToListAsync<T>(IQueryable<T> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
            {
                return null;
            }

            return await Task.Run(() =>
            {
                return source.ToList();
            });
        }
    }
}
