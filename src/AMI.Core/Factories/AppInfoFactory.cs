using AMI.Core.Entities.Models;
using AMI.Core.Helpers;

namespace AMI.Core.Factories
{
    /// <summary>
    /// A factory for the application information model.
    /// </summary>
    /// <seealso cref="IAppInfoFactory" />
    public class AppInfoFactory : IAppInfoFactory
    {
        /// <summary>
        /// Creates a new application information model instance.
        /// </summary>
        /// <returns>
        /// The application information model instance.
        /// </returns>
        public AppInfo Create()
        {
            return new AppInfo(ReflectionHelper.GetAssemblyName(), ReflectionHelper.GetAssemblyVersion());
        }
    }
}
