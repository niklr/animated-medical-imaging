using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
        /// Begins a new transaction.
        /// </summary>
        void BeginTransaction();

        /// <summary>
        /// Commits the transaction.
        /// </summary>
        void CommitTransaction();

        /// <summary>
        /// Specifies related entities to include in the query results.
        /// </summary>
        /// <typeparam name="T">The type of entity being queried.</typeparam>
        /// <param name="source">The source query.</param>
        /// <param name="navigationPropertyPath">A lambda expression representing the navigation property to be included.</param>
        /// <returns>A new query with the related data included.</returns>
        IQueryable<T> Include<T>(IQueryable<T> source, Expression<Func<T, bool>> navigationPropertyPath)
            where T : class;

        /// <summary>
        /// Rolls the transaction back.
        /// </summary>
        void RollBackTransaction();

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
        /// Creates a list from a queryalbe by enumerating it asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <param name="source">The source to create a list from.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result contains a list that contains elements from the input sequence.</returns>
        Task<List<T>> ToListAsync<T>(IQueryable<T> source, CancellationToken cancellationToken = default);
    }
}
