using System;
using AMI.Core.Factories;
using AMI.Core.Repositories;
using AMI.Infrastructure.Repositories;

namespace AMI.Infrastructure.Factories
{
    /// <summary>
    /// A factory to create repositories.
    /// </summary>
    public class RepositoryFactory : IRepositoryFactory
    {
        private readonly IAmiUnitOfWork context;

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryFactory"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public RepositoryFactory(IAmiUnitOfWork context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <inheritdoc/>
        public IUserRepository CreateUserRepository()
        {
            return new UserRepository(context.UserRepository);
        }
    }
}
