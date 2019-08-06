using System;
using AMI.Core.Repositories;
using AMI.Domain.Entities;

namespace AMI.Infrastructure.Repositories
{
    /// <summary>
    /// A repository for user entities.
    /// </summary>
    public class UserRepository : BaseRepository<UserEntity>, IUserRepository
    {
        private readonly IRepository<UserEntity> repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRepository"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        public UserRepository(IRepository<UserEntity> repository)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        /// <inheritdoc/>
        protected override IRepository<UserEntity> Repository
        {
            get
            {
                return repository;
            }
        }
    }
}
