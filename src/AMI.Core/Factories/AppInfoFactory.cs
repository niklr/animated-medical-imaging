using System;
using AMI.Core.Entities.Models;
using AMI.Core.Helpers;
using RNS.Framework.Semantic;

namespace AMI.Core.Factories
{
    /// <summary>
    /// A factory to create a model containing information about the application.
    /// </summary>
    /// <seealso cref="IAppInfoFactory" />
    public class AppInfoFactory : IAppInfoFactory
    {
        /// <inheritdoc/>
        public AppInfo Create()
        {
            var semVer = new SemanticVersion(ReflectionHelper.GetAssemblyVersion());
            return new AppInfo(ReflectionHelper.GetAssemblyName(), semVer.ToNormalizedString());
        }

        /// <inheritdoc/>
        public AppInfo Create(Type type)
        {
            var semVer = new SemanticVersion(ReflectionHelper.GetAssemblyVersion(type));
            return new AppInfo(ReflectionHelper.GetAssemblyName(type), semVer.ToNormalizedString());
        }
    }
}
