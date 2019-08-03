using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using RNS.Framework.Collections;

namespace AMI.Persistence.EntityFramework.Shared.Repositories
{
    /// <summary>
    /// An implementation of the repository using DbSet.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <seealso cref="IRepository{T}" />
    public class DbSetRepository<T> : IRepository<T>
        where T : class
    {
        private readonly DbSet<T> dbSet;

        /// <summary>
        /// Initializes a new instance of the <see cref="DbSetRepository{T}"/> class.
        /// </summary>
        /// <param name="dbSet">The database set.</param>
        public DbSetRepository(DbSet<T> dbSet)
        {
            this.dbSet = dbSet ?? throw new ArgumentNullException(nameof(dbSet));
        }

        /// <inheritdoc/>
        public void Add(T entity)
        {
            dbSet.Add(entity);
        }

        /// <inheritdoc/>
        public void AddRange(IEnumerable<T> entities)
        {
            dbSet.AddRange(entities);
        }

        /// <inheritdoc/>
        public Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            return dbSet.AddRangeAsync(entities, cancellationToken);
        }

        /// <inheritdoc/>
        public void Attach(T entity)
        {
            dbSet.Attach(entity);
        }

        /// <inheritdoc/>
        public int Count()
        {
            return dbSet.Count();
        }

        /// <inheritdoc/>
        public int Count(Expression<Func<T, bool>> predicate)
        {
            return dbSet.Count(predicate);
        }

        /// <inheritdoc/>
        public Task<int> CountAsync(CancellationToken cancellationToken = default)
        {
            return dbSet.CountAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public Task<int> CountAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return dbSet.CountAsync(predicate, cancellationToken);
        }

        /// <inheritdoc/>
        public IEnumerable<T> Get(Expression<Func<T, bool>> predicate)
        {
            return GetQuery(predicate).ToList();
        }

        /// <inheritdoc/>
        public IPaginateEnumerable<T> Get<TKey>(Expression<Func<T, TKey>> keySelector, Expression<Func<T, bool>> predicate, int pageIndex, int pageSize)
        {
            var query = GetQuery(predicate).OrderBy(keySelector).Skip(pageIndex * pageSize).Take(pageSize);
            return PaginateList<T>.Create(query, pageIndex, pageSize, Count(predicate));
        }

        /// <inheritdoc/>
        public IPaginateEnumerable<T> Get<TKey>(Expression<Func<T, TKey>> keySelector, int pageIndex, int pageSize)
        {
            var query = GetQuery().OrderBy(keySelector).Skip(pageIndex * pageSize).Take(pageSize);
            return PaginateList<T>.Create(query, pageIndex, pageSize, Count());
        }

        /// <inheritdoc/>
        public T GetFirstOrDefault(Expression<Func<T, bool>> predicate)
        {
            return GetQuery(predicate).FirstOrDefault();
        }

        /// <inheritdoc/>
        public Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return GetQuery(predicate).FirstOrDefaultAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public IQueryable<T> GetQuery()
        {
            return dbSet;
        }

        /// <inheritdoc/>
        public IQueryable<T> GetQuery(Expression<Func<T, bool>> predicate)
        {
            return GetQuery().Where(predicate);
        }

        /// <inheritdoc/>
        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }

        /// <inheritdoc/>
        public void RemoveRange(Expression<Func<T, bool>> predicate)
        {
            dbSet.RemoveRange(Get(predicate));
        }

        /// <inheritdoc/>
        public void Update(T entity)
        {
            dbSet.Update(entity);
        }

        /// <inheritdoc/>
        public void UpdateRange(IEnumerable<T> entities)
        {
            dbSet.UpdateRange(entities);
        }
    }
}
