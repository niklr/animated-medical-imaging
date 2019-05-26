using AMI.Core.Entities.Models;

namespace AMI.Core.Factories
{
    /// <summary>
    /// A factory for the application information model.
    /// </summary>
    public interface IAppInfoFactory
    {
        /// <summary>
        /// Creates a new application information model instance.
        /// </summary>
        /// <returns>The application information model instance.</returns>
        AppInfo Create();
    }
}
