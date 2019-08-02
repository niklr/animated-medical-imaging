using System;
using AMI.Core.Constants;
using AMI.Core.IO.Serializers;
using AMI.Core.Modules;
using AMI.Core.Providers;
using AMI.Core.Repositories;

namespace AMI.Infrastructure.Modules
{
    /// <summary>
    /// A module to group common services used by query handlers.
    /// </summary>
    /// <seealso cref="IQueryHandlerModule" />
    public class QueryHandlerModule : IQueryHandlerModule
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QueryHandlerModule"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="constants">The application constants.</param>
        /// <param name="principalProvider">The principal provider.</param>
        /// <param name="serializer">The JSON serializer.</param>
        public QueryHandlerModule(
            IAmiUnitOfWork context,
            IApplicationConstants constants,
            ICustomPrincipalProvider principalProvider,
            IDefaultJsonSerializer serializer)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
            Constants = constants ?? throw new ArgumentNullException(nameof(constants));
            PrincipalProvider = principalProvider ?? throw new ArgumentNullException(nameof(principalProvider));
            Serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
        }

        /// <inheritdoc/>
        public IAmiUnitOfWork Context { get; private set; }

        /// <inheritdoc/>
        public IApplicationConstants Constants { get; private set; }

        /// <inheritdoc/>
        public ICustomPrincipalProvider PrincipalProvider { get; private set; }

        /// <inheritdoc/>
        public IDefaultJsonSerializer Serializer { get; private set; }
    }
}
