using System;
using AMI.Domain.Exceptions;
using Microsoft.Extensions.Options;
using RNS.Framework.Extensions.ObjectExtensions;
using RNS.Framework.Tools;

namespace AMI.Core.Configurations
{
    /// <summary>
    /// The abstract implementation of the base configuration.
    /// </summary>
    /// <typeparam name="T">The type of options being requested.</typeparam>
    /// <seealso cref="IBaseConfiguration{T}" />
    public abstract class BaseConfiguration<T> : IBaseConfiguration<T>
        where T : class, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseConfiguration{T}"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <exception cref="ArgumentNullException">options</exception>
        /// <exception cref="UnexpectedNullException">options - T</exception>
        public BaseConfiguration(IOptions<T> options)
        {
            Ensure.ArgumentNotNull(options, nameof(options));

            if (options.Value == null)
            {
                throw new UnexpectedNullException(nameof(options), nameof(T));
            }

            Options = options.Value;
        }

        /// <summary>
        /// Gets the configuration options.
        /// </summary>
        protected T Options { get; }

        /// <inheritdoc/>
        public T Clone()
        {
            return Options.DeepClone();
        }
    }
}
