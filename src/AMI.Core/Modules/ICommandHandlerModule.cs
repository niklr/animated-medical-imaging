using AMI.Core.Providers;
using AMI.Core.Repositories;
using AMI.Core.Services;

namespace AMI.Core.Modules
{
    /// <summary>
    /// An interface representing a module for command handlers.
    /// </summary>
    public interface ICommandHandlerModule
    {
        /// <summary>
        /// Gets the auditing service.
        /// </summary>
        IAuditService Audit { get; }

        /// <summary>
        /// Gets the context.
        /// </summary>
        IAmiUnitOfWork Context { get; }

        /// <summary>
        /// Gets the events service.
        /// </summary>
        IEventService Events { get; }

        /// <summary>
        /// Gets the gateway service.
        /// </summary>
        IGatewayService Gateway { get; }

        /// <summary>
        /// Gets the auth service.
        /// </summary>
        IAuthService AuthService { get; }

        /// <summary>
        /// Gets the principal provider.
        /// </summary>
        ICustomPrincipalProvider PrincipalProvider { get; }
    }
}
