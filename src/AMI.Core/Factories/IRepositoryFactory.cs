using AMI.Core.Repositories;

namespace AMI.Core.Factories
{
    /// <summary>
    /// A factory to create repositories.
    /// </summary>
    public interface IRepositoryFactory
    {
        /// <summary>
        /// Creates a user repository.
        /// </summary>
        /// <returns>The user repository.</returns>
        IUserRepository CreateUserRepository();
    }
}
