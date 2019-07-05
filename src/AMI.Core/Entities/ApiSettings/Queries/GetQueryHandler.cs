using System;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Configurations;
using MediatR;

namespace AMI.Core.Entities.ApiSettings.Queries
{
    /// <summary>
    /// A handler for the query to get the application settings.
    /// </summary>
    public class GetQueryHandler : IRequestHandler<GetQuery, Models.ApiSettings>
    {
        private readonly IApiConfiguration configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetQueryHandler"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <exception cref="ArgumentNullException">configuration</exception>
        public GetQueryHandler(IApiConfiguration configuration)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        /// <summary>
        /// Handles the specified request.
        /// </summary>
        /// <param name="request">The query request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result contains the application settings.</returns>
        public Task<Models.ApiSettings> Handle(GetQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(configuration.ToModel());
        }
    }
}
