using System;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Configurations;
using MediatR;

namespace AMI.Core.Entities.AppOptions.Queries
{
    /// <summary>
    /// A handler for the query to get the application options.
    /// </summary>
    public class GetQueryHandler : IRequestHandler<GetQuery, Models.AppOptions>
    {
        private readonly IAppConfiguration configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetQueryHandler"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <exception cref="ArgumentNullException">configuration</exception>
        public GetQueryHandler(IAppConfiguration configuration)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        /// <summary>
        /// Handles the specified request.
        /// </summary>
        /// <param name="request">The query request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result contains the application options.</returns>
        public Task<Models.AppOptions> Handle(GetQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(configuration.Clone());
        }
    }
}
