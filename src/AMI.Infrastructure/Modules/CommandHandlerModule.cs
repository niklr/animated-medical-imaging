using System;
using AMI.Core.Modules;
using AMI.Core.Providers;
using AMI.Core.Repositories;
using AMI.Core.Services;

namespace AMI.Infrastructure.Modules
{
    /// <summary>
    /// A module to group common services used by command handlers.
    /// </summary>
    /// <seealso cref="ICommandHandlerModule" />
    public class CommandHandlerModule : ICommandHandlerModule
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandHandlerModule"/> class.
        /// </summary>
        /// <param name="auditService">The auditing service.</param>
        /// <param name="context">The context.</param>
        /// <param name="eventService">The events service.</param>
        /// <param name="gateway">The gateway service.</param>
        /// <param name="authService">The identity service.</param>
        /// <param name="principalProvider">The principal provider.</param>
        public CommandHandlerModule(
            IAuditService auditService,
            IAmiUnitOfWork context,
            IEventService eventService,
            IGatewayService gateway,
            IAuthService authService,
            ICustomPrincipalProvider principalProvider)
        {
            Audit = auditService ?? throw new ArgumentNullException(nameof(auditService));
            Context = context ?? throw new ArgumentNullException(nameof(context));
            Events = eventService ?? throw new ArgumentNullException(nameof(eventService));
            Gateway = gateway ?? throw new ArgumentNullException(nameof(gateway));
            AuthService = authService ?? throw new ArgumentNullException(nameof(authService));
            PrincipalProvider = principalProvider ?? throw new ArgumentNullException(nameof(principalProvider));
        }

        /// <inheritdoc/>
        public IAuditService Audit { get; private set; }

        /// <inheritdoc/>
        public IAmiUnitOfWork Context { get; private set; }

        /// <inheritdoc/>
        public IEventService Events { get; private set; }

        /// <inheritdoc/>
        public IGatewayService Gateway { get; private set; }

        /// <inheritdoc/>
        public IAuthService AuthService { get; private set; }

        /// <inheritdoc/>
        public ICustomPrincipalProvider PrincipalProvider { get; private set; }
    }
}
