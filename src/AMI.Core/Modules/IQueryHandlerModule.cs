using AMI.Core.Constants;
using AMI.Core.IO.Serializers;
using AMI.Core.Providers;
using AMI.Core.Repositories;

namespace AMI.Core.Modules
{
    /// <summary>
    /// An interface representing a module for query handlers.
    /// </summary>
    public interface IQueryHandlerModule
    {
        /// <summary>
        /// Gets the context.
        /// </summary>
        IAmiUnitOfWork Context { get; }

        /// <summary>
        /// Gets the application constants.
        /// </summary>
        IApplicationConstants Constants { get; }

        /// <summary>
        /// Gets the principal provider.
        /// </summary>
        ICustomPrincipalProvider PrincipalProvider { get; }

        /// <summary>
        /// Gets the JSON serializer.
        /// </summary>
        IDefaultJsonSerializer Serializer { get; }
    }
}
