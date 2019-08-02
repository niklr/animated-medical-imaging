using System;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Entities.Shared.Queries;
using AMI.Core.Factories;
using AMI.Core.Modules;

namespace AMI.Core.Entities.AppInfo.Queries
{
    /// <summary>
    /// A query handler to get the application information.
    /// </summary>
    public class GetQueryHandler : BaseQueryRequestHandler<GetQuery, Models.AppInfo>
    {
        private readonly IAppInfoFactory factory;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetQueryHandler"/> class.
        /// </summary>
        /// <param name="module">The query handler module.</param>
        /// <param name="factory">The factory.</param>
        /// <exception cref="ArgumentNullException">factory</exception>
        public GetQueryHandler(IQueryHandlerModule module, IAppInfoFactory factory)
            : base(module)
        {
            this.factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }

        /// <inheritdoc/>
        protected override Task<Models.AppInfo> ProtectedHandleAsync(GetQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(factory.Create());
        }
    }
}
