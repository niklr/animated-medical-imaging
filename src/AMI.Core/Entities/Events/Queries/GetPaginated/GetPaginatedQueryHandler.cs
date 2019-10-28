using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Entities.Models;
using AMI.Core.Entities.Shared.Queries;
using AMI.Core.Modules;
using AMI.Domain.Entities;
using AMI.Domain.Enums;
using AMI.Domain.Exceptions;
using RNS.Framework.Search;

namespace AMI.Core.Entities.Events.Queries.GetPaginated
{
    /// <summary>
    /// A query handler to get a list of paginated events.
    /// </summary>
    public class GetPaginatedQueryHandler : BaseQueryRequestHandler<GetPaginatedQuery, PaginationResultModel<EventModel>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetPaginatedQueryHandler"/> class.
        /// </summary>
        /// <param name="module">The query handler module.</param>
        public GetPaginatedQueryHandler(IQueryHandlerModule module)
            : base(module)
        {
        }

        /// <inheritdoc/>
        protected override async Task<PaginationResultModel<EventModel>> ProtectedHandleAsync(GetPaginatedQuery request, CancellationToken cancellationToken)
        {
            var principal = PrincipalProvider.GetPrincipal();
            if (principal == null)
            {
                throw new ForbiddenException("Not authenticated");
            }

            Expression<Func<EventEntity, bool>> expression = PredicateBuilder.True<EventEntity>();
            if (!principal.IsInRole(RoleType.Administrator))
            {
                expression = expression.And(e => e.UserId == principal.Identity.Name);
            }

            int total = await Context.EventRepository.CountAsync(expression, cancellationToken);

            var query = Context.EventRepository
                .GetQuery(expression)
                .OrderByDescending(e => e.CreatedDate)
                .Skip(request.Page * request.Limit)
                .Take(request.Limit);

            var entities = await Context.ToListAsync(query, cancellationToken);
            var models = entities.Select(e => EventModel.Create(e, Serializer));

            return PaginationResultModel<EventModel>.Create(models, request.Page, request.Limit, total);
        }
    }
}
