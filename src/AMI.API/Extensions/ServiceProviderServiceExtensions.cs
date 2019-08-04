using System;
using AMI.Domain.Exceptions;
using Microsoft.Extensions.DependencyInjection;

namespace AMI.API.Extensions.ServiceProviderServiceExtensions
{
    /// <summary>
    /// Extensions related to <see cref="IServiceProvider"/>
    /// </summary>
    public static class ServiceProviderServiceExtensions
    {
        /// <summary>
        /// Get service of type T from the <see cref="IServiceProvider"/>.
        /// Throws <see cref="UnexpectedNullException"/> if the service could not be retrieved.
        /// </summary>
        /// <typeparam name="T">The type of service object to get.</typeparam>
        /// <param name="serviceProvider">The service provider.</param>
        /// <returns>A service object of type T or null if there is no such service.</returns>
        /// <exception cref="UnexpectedNullException">Thrown if the service could not be retrieved.</exception>
        public static T EnsureGetService<T>(this IServiceProvider serviceProvider)
        {
            var service = serviceProvider.GetService<T>();
            if (service == null)
            {
                throw new UnexpectedNullException($"{nameof(T)} could not be retrieved.");
            }

            return service;
        }
    }
}
