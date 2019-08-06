using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using RNS.Framework.Collections;

namespace AMI.Core.Repositories
{
    /// <summary>
    /// The base implementation all repositories have in common.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <seealso cref="IRepository{TEntity}" />
    public abstract class BaseRepository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseRepository{TEntity}"/> class.
        /// </summary>
        public BaseRepository()
        {
        }

        /// <summary>
        /// Gets the repository.
        /// </summary>
        /// <returns>The repository.</returns>
        protected abstract IRepository<TEntity> Repository { get; }

        /// <inheritdoc/>
        public void Add(TEntity entity)
        {
            Repository.Add(entity);
        }

        /// <inheritdoc/>
        public void AddRange(IEnumerable<TEntity> entities)
        {
            Repository.AddRange(entities);
        }

        /// <inheritdoc/>
        public Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            return Repository.AddRangeAsync(entities, cancellationToken);
        }

        /// <inheritdoc/>
        public void Attach(TEntity entity)
        {
            Repository.Attach(entity);
        }

        /// <inheritdoc/>
        public int Count()
        {
            return Repository.Count();
        }

        /// <inheritdoc/>
        public int Count(Expression<Func<TEntity, bool>> predicate)
        {
            return Repository.Count(predicate);
        }

        /// <inheritdoc/>
        public Task<int> CountAsync(CancellationToken cancellationToken = default)
        {
            return Repository.CountAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return Repository.CountAsync(predicate, cancellationToken);
        }

        /// <inheritdoc/>
        public IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> predicate)
        {
            return Repository.Get(predicate);
        }

        /// <inheritdoc/>
        public IPaginateEnumerable<TEntity> Get<TKey>(Expression<Func<TEntity, TKey>> keySelector, Expression<Func<TEntity, bool>> predicate, int pageIndex, int pageSize)
        {
            return Repository.Get(keySelector, predicate, pageIndex, pageSize);
        }

        /// <inheritdoc/>
        public IPaginateEnumerable<TEntity> Get<TKey>(Expression<Func<TEntity, TKey>> keySelector, int pageIndex, int pageSize)
        {
            return Repository.Get(keySelector, pageIndex, pageSize);
        }

        /// <inheritdoc/>
        public TEntity GetFirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return Repository.GetFirstOrDefault(predicate);
        }

        /// <inheritdoc/>
        public Task<TEntity> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return Repository.GetFirstOrDefaultAsync(predicate, cancellationToken);
        }

        /// <inheritdoc/>
        public IQueryable<TEntity> GetQuery()
        {
            return Repository.GetQuery();
        }

        /// <inheritdoc/>
        public IQueryable<TEntity> GetQuery(Expression<Func<TEntity, bool>> predicate)
        {
            return Repository.GetQuery(predicate);
        }

        /// <inheritdoc/>
        public void Remove(TEntity entity)
        {
            Repository.Remove(entity);
        }

        /// <inheritdoc/>
        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            Repository.RemoveRange(entities);
        }

        /// <inheritdoc/>
        public void Update(TEntity entity)
        {
            Repository.Update(entity);
        }

        /// <inheritdoc/>
        public void UpdateRange(IEnumerable<TEntity> entities)
        {
            Repository.UpdateRange(entities);
        }
    }
}
