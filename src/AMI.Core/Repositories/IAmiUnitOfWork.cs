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
        /// Gets the audit event repository.
        /// </summary>
        IRepository<AuditEventEntity> AuditEventRepository { get; }

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

        /// <summary>
        /// Gets the token repository.
        /// </summary>
        IRepository<TokenEntity> TokenRepository { get; }

        /// <summary>
        /// Gets the user repository.
        /// </summary>
        IRepository<UserEntity> UserRepository { get; }

        /// <summary>
        /// Applies any pending migrations for the context to the database.
        /// Will create the database if it does not already exist.
        /// </summary>
        void Migrate();
    }
}
