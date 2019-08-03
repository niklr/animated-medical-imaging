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
    /// The mock implementation of the repository.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <seealso cref="IRepository{T}" />
    public class MockRepository<T> : IRepository<T>
        where T : class
    {
        private readonly IList<T> entities;

        /// <summary>
        /// Initializes a new instance of the <see cref="MockRepository{T}"/> class.
        /// </summary>
        /// <param name="entities">The entities list.</param>
        public MockRepository(IList<T> entities)
        {
            this.entities = entities ?? throw new ArgumentNullException(nameof(entities));
        }

        /// <inheritdoc/>
        public void Add(T entity)
        {
            entities.Add(entity);
        }

        /// <inheritdoc/>
        public void AddRange(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                Add(entity);
            }
        }

        /// <inheritdoc/>
        public Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            AddRange(entities);

            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public void Attach(T entity)
        {
        }

        /// <inheritdoc/>
        public int Count()
        {
            return entities.Count;
        }

        /// <inheritdoc/>
        public int Count(Expression<Func<T, bool>> predicate)
        {
            return GetQuery(predicate).Count();
        }

        /// <inheritdoc/>
        public async Task<int> CountAsync(CancellationToken cancellationToken = default)
        {
            return await Task.Run(() => { return Count(); });
        }

        /// <inheritdoc/>
        public async Task<int> CountAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await Task.Run(() => { return Count(predicate); });
        }

        /// <inheritdoc/>
        public IEnumerable<T> Get(Expression<Func<T, bool>> predicate)
        {
            return GetQuery(predicate).AsEnumerable();
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
        public async Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await Task.Run(() => { return GetQuery(predicate).FirstOrDefault(); });
        }

        /// <inheritdoc/>
        public IQueryable<T> GetQuery()
        {
            return entities.AsQueryable();
        }

        /// <inheritdoc/>
        public IQueryable<T> GetQuery(Expression<Func<T, bool>> predicate)
        {
            return GetQuery().Where(predicate);
        }

        /// <inheritdoc/>
        public void Remove(T entity)
        {
            entities.Remove(entity);
        }

        /// <inheritdoc/>
        public void RemoveRange(Expression<Func<T, bool>> predicate)
        {
            List<T> entities = GetQuery(predicate).ToList();
            foreach (T entity in entities)
            {
                Remove(entity);
            }
        }

        /// <inheritdoc/>
        public void Update(T entity)
        {
        }

        /// <inheritdoc/>
        public void UpdateRange(IEnumerable<T> entities)
        {
        }
    }
}
