using System;
using AMI.Core.Entities.Models;

namespace AMI.Core.Factories
{
    /// <summary>
    /// A factory to create a model containing information about the application.
    /// </summary>
    public interface IAppInfoFactory
    {
        /// <summary>
        /// Creates a model containing information about the application.
        /// </summary>
        /// <returns>The model containing information about the application.</returns>
        AppInfoModel Create();

        /// <summary>
        /// Creates a model containing information about the application based on the specified type.
        /// </summary>
        /// <param name="type">An object representing a type in the assembly that will be used to derive the application information.</param>
        /// <returns>The model containing information about the application.</returns>
        AppInfoModel Create(Type type);
    }
}
