using System;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Factories;
using MediatR;

namespace AMI.Core.Entities.AppInfo.Queries
{
    /// <summary>
    /// A handler for the query to get the application information.
    /// </summary>
    public class GetQueryHandler : IRequestHandler<GetQuery, Models.AppInfo>
    {
        private readonly IAppInfoFactory factory;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetQueryHandler"/> class.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <exception cref="ArgumentNullException">factory</exception>
        public GetQueryHandler(IAppInfoFactory factory)
        {
            this.factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }

        /// <summary>
        /// Handles the specified request.
        /// </summary>
        /// <param name="request">The query request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result contains the application information.</returns>
        public Task<Models.AppInfo> Handle(GetQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(factory.Create());
        }
    }
}
