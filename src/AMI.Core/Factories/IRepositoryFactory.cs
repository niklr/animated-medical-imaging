using AMI.Core.Repositories;

namespace AMI.Core.Factories
{
    /// <summary>
    /// A factory to create repositories.
    /// </summary>
    public interface IRepositoryFactory
    {
        /// <summary>
        /// Creates the application log repository.
        /// </summary>
        /// <returns>The application repository.</returns>
        IAppLogRepository CreateAppLogRepository();

        /// <summary>
        /// Creates a user repository.
        /// </summary>
        /// <returns>The user repository.</returns>
        IUserRepository CreateUserRepository();
    }
}
