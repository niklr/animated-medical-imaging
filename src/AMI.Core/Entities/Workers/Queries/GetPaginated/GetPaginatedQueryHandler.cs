using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Entities.Models;
using AMI.Core.Entities.Shared.Queries;
using AMI.Core.Modules;
using AMI.Core.Services;

namespace AMI.Core.Entities.Workers.Queries.GetPaginated
{
    /// <summary>
    /// A query handler to get a list of paginated workers.
    /// </summary>
    public class GetPaginatedQueryHandler : BaseQueryRequestHandler<GetPaginatedQuery, PaginationResultModel<BaseWorkerModel>>
    {
        private readonly IWorkerService workerService;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetPaginatedQueryHandler"/> class.
        /// </summary>
        /// <param name="module">The query handler module.</param>
        /// <param name="workerService">The worker service.</param>
        public GetPaginatedQueryHandler(IQueryHandlerModule module, IWorkerService workerService)
            : base(module)
        {
            this.workerService = workerService ?? throw new ArgumentNullException(nameof(workerService));
        }

        /// <inheritdoc/>
        protected override async Task<PaginationResultModel<BaseWorkerModel>> ProtectedHandleAsync(GetPaginatedQuery request, CancellationToken cancellationToken)
        {
            var workers = workerService.GetWorkers();
            int total = workers.Count;

            var result = workers
                .OrderBy(e => e.WorkerName)
                .Skip(request.Page * request.Limit)
                .Take(request.Limit)
                .Select(e => BaseWorkerModel.Create(e));

            return await Task.Run(() => { return PaginationResultModel<BaseWorkerModel>.Create(result, request.Page, request.Limit, total); });
        }
    }
}
