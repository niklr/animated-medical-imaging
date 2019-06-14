using AMI.Domain.Entities;

namespace AMI.Core.Repositories
{
    /// <summary>
    /// An interface for the Unit of Work pattern of the application.
    /// </summary>
    /// <seealso cref="IUnitOfWork" />
    public interface IAmiUnitOfWork : IUnitOfWork
    {
        /// <summary>
        /// Gets the object repository.
        /// </summary>
        IRepository<ObjectEntity> ObjectRepository { get; }

        /// <summary>
        /// Gets the result repository.
        /// </summary>
        IRepository<ResultEntity> ResultRepository { get; }

        /// <summary>
        /// Gets the task repository.
        /// </summary>
        IRepository<TaskEntity> TaskRepository { get; }
    }
}
