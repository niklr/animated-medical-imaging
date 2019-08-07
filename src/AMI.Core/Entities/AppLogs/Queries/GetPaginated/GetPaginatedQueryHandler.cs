using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Entities.Models;
using AMI.Core.Entities.Shared.Queries;
using AMI.Core.Modules;
using AMI.Core.Repositories;

namespace AMI.Core.Entities.AppLogs.Queries.GetPaginated
{
    /// <summary>
    /// A query handler to get a list of paginated application logs.
    /// </summary>
    public class GetPaginatedQueryHandler : BaseQueryRequestHandler<GetPaginatedQuery, PaginationResultModel<AppLogModel>>
    {
        private readonly IAppLogRepository appLogRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetPaginatedQueryHandler"/> class.
        /// </summary>
        /// <param name="module">The query handler module.</param>
        /// <param name="appLogRepository">The application log repository.</param>
        public GetPaginatedQueryHandler(IQueryHandlerModule module, IAppLogRepository appLogRepository)
            : base(module)
        {
            this.appLogRepository = appLogRepository ?? throw new ArgumentNullException(nameof(appLogRepository));
        }

        /// <inheritdoc/>
        protected override async Task<PaginationResultModel<AppLogModel>> ProtectedHandleAsync(GetPaginatedQuery request, CancellationToken cancellationToken)
        {
            int total = await appLogRepository.CountAsync(cancellationToken);

            var result = appLogRepository
                .GetQuery()
                .OrderByDescending(e => e.Timestamp)
                .Skip(request.Page * request.Limit)
                .Take(request.Limit)
                .Select(e => AppLogModel.Create(e));

            return PaginationResultModel<AppLogModel>.Create(result, request.Page, request.Limit, total);
        }
    }
}
