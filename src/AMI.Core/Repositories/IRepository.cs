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
    /// Represents the operations that can be executed againt a repository.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public interface IRepository<TEntity>
        where TEntity : class
    {
        /// <summary>
        /// Adds the given entity to the repository.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        void Add(TEntity entity);

        /// <summary>
        /// Attaches the given entity to the repository.
        /// </summary>
        /// <param name="entity">The entity to attach.</param>
        void Attach(TEntity entity);

        /// <summary>
        /// Counts entities in the repository.
        /// </summary>
        /// <returns>The amount of entities.</returns>
        int Count();

        /// <summary>
        /// Counts entities in the repository matching the given criteria.
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>The amount of entities matching the given criteria.</returns>
        int Count(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Counts entities in the repository asynchronous.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The amount of entities.</returns>
        Task<int> CountAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Counts entities in the repository matching the given criteria asynchronous.
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The amount of entities matching the given criteria.</returns>
        Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets entities filtered based on a predicate.
        /// </summary>
        /// <param name="predicate">A function to test each entity for a condition.</param>
        /// <returns>Entities from the input sequence that satisfy the condition specified by predicate.</returns>
        IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Gets a paginated list of entities sorted in ascending order according to a key and filtered based on a predicate.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="keySelector">A function to extract a key from an entity.</param>
        /// <param name="predicate">A function to test each entity for a condition.</param>
        /// <param name="pageIndex">The index of the page.</param>
        /// <param name="pageSize">The size of the page.</param>
        /// <returns>A paginated list whose entities are sorted according to a key and filtered based on a predicate.</returns>
        IPaginateEnumerable<TEntity> Get<TKey>(
            Expression<Func<TEntity, TKey>> keySelector, Expression<Func<TEntity, bool>> predicate, int pageIndex, int pageSize);

        /// <summary>
        /// Gets a paginated list of entities sorted in ascending order according to a key.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="keySelector">A function to extract a key from an entity.</param>
        /// <param name="pageIndex">The index of the page.</param>
        /// <param name="pageSize">The size of the page.</param>
        /// <returns>A paginated list whose entities are sorted according to a key.</returns>
        IPaginateEnumerable<TEntity> Get<TKey>(Expression<Func<TEntity, TKey>> keySelector, int pageIndex, int pageSize);

        /// <summary>
        /// Gets the first entity of a sequence, or a default value if the sequence contains no entities.
        /// </summary>
        /// <param name="predicate">A function to test each entity for a condition.</param>
        /// <returns>A default value if source is empty; otherwise, the first entity in source.</returns>
        TEntity GetFirstOrDefault(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Gets the first entity of a sequence, or a default value if the sequence contains no entities asynchronous.
        /// </summary>
        /// <param name="predicate">A function to test each entity for a condition.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A default value if source is empty; otherwise, the first entity in source.</returns>
        Task<TEntity> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets a queryable set for the given entity type.
        /// </summary>
        /// <returns>A queryable set for the given entity type.</returns>
        IQueryable<TEntity> GetQuery();

        /// <summary>
        /// Gets a queryable set for the given entity type filtered based on a predicate.
        /// </summary>
        /// <param name="predicate">A function to extract a key from an entity.</param>
        /// <returns>A queryable set for the given entity type filtered based on a predicate.</returns>
        IQueryable<TEntity> GetQuery(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Removes the given entity from the repository.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void Remove(TEntity entity);

        /// <summary>
        /// Removes one or many entities matching the given predicate.
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        void RemoveRange(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Updates changes of the existing entity.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        void Update(TEntity entity);
    }
}
