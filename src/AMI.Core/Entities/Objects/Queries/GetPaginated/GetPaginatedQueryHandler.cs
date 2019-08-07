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

namespace AMI.Core.Entities.Objects.Queries.GetPaginated
{
    /// <summary>
    /// A query handler to get a list of paginated objects.
    /// </summary>
    public class GetPaginatedQueryHandler : BaseQueryRequestHandler<GetPaginatedQuery, PaginationResultModel<ObjectModel>>
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
        protected override async Task<PaginationResultModel<ObjectModel>> ProtectedHandleAsync(GetPaginatedQuery request, CancellationToken cancellationToken)
        {
            var principal = PrincipalProvider.GetPrincipal();
            if (principal == null)
            {
                throw new ForbiddenException("Not authenticated");
            }

            Expression<Func<ObjectEntity, bool>> expression = PredicateBuilder.True<ObjectEntity>();
            if (!principal.IsInRole(RoleType.Administrator))
            {
                expression = expression.And(e => e.UserId == principal.Identity.Name);
            }

            int total = await Context.ObjectRepository.CountAsync(cancellationToken);

            var query = Context.ObjectRepository
                .GetQuery(expression)
                .Select(e => new
                {
                    Object = e,
                    Tasks = e.Tasks.OrderByDescending(e1 => e1.CreatedDate).Take(request.Limit),
                    Results = e.Tasks.OrderByDescending(e1 => e1.CreatedDate).Select(e1 => e1.Result).Take(request.Limit)
                })
                .OrderByDescending(e => e.Object.CreatedDate)
                .Skip(request.Page * request.Limit)
                .Take(request.Limit);

            var entities = await Context.ToListAsync(query, cancellationToken);
            var models = entities.Select(e => ObjectModel.Create(e.Object, e.Tasks, e.Results, Serializer));

            return PaginationResultModel<ObjectModel>.Create(models, request.Page, request.Limit, total);
        }
    }
}
