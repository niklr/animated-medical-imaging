using System;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Configurations;
using AMI.Core.Entities.Shared.Queries;
using AMI.Core.Modules;

namespace AMI.Core.Entities.ApiOptions.Queries
{
    /// <summary>
    /// A query handler to get the application options.
    /// </summary>
    public class GetQueryHandler : BaseQueryRequestHandler<GetQuery, Models.ApiOptions>
    {
        private readonly IApiConfiguration configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetQueryHandler"/> class.
        /// </summary>
        /// <param name="module">The query handler module.</param>
        /// <param name="configuration">The configuration.</param>
        public GetQueryHandler(IQueryHandlerModule module, IApiConfiguration configuration)
            : base(module)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        /// <inheritdoc/>
        protected override Task<Models.ApiOptions> ProtectedHandleAsync(GetQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(configuration.Clone());
        }
    }
}
