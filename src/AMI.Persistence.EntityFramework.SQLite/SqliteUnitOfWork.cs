using AMI.Core.Repositories;
using AMI.Domain.Entities;
using AMI.Persistence.EntityFramework.Shared.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AMI.Persistence.EntityFramework.SQLite
{
    /// <summary>
    /// The EntityFramework SQLite implementation of the Unit of Work pattern.
    /// </summary>
    /// <seealso cref="UnitOfWork" />
    /// <seealso cref="IAmiUnitOfWork" />
    public class SqliteUnitOfWork : UnitOfWork, IAmiUnitOfWork
    {
        private readonly SqliteDbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="SqliteUnitOfWork"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public SqliteUnitOfWork(SqliteDbContext context)
            : base(context)
        {
            this.context = context;
        }

        /// <inheritdoc/>
        public IRepository<ObjectEntity> ObjectRepository
        {
            get
            {
                return new DbSetRepository<ObjectEntity>(context.Objects);
            }
        }

        /// <inheritdoc/>
        public IRepository<ResultEntity> ResultRepository
        {
            get
            {
                return new DbSetRepository<ResultEntity>(context.Results);
            }
        }

        /// <inheritdoc/>
        public IRepository<TaskEntity> TaskRepository
        {
            get
            {
                return new DbSetRepository<TaskEntity>(context.Tasks);
            }
        }

        /// <inheritdoc/>
        public IRepository<TokenEntity> TokenRepository
        {
            get
            {
                return new DbSetRepository<TokenEntity>(context.Tokens);
            }
        }

        /// <inheritdoc/>
        public IRepository<UserEntity> UserRepository
        {
            get
            {
                return new DbSetRepository<UserEntity>(context.Users);
            }
        }

        /// <inheritdoc/>
        public void Migrate()
        {
            context.Database.Migrate();
        }
    }
}
