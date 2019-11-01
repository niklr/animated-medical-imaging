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

namespace AMI.Core.Entities.Events.Queries.GetById
{
    /// <summary>
    /// A query handler to get an entity by its identifier.
    /// </summary>
    public class GetByIdQueryHandler : BaseQueryRequestHandler<GetByIdQuery, EventModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetByIdQueryHandler"/> class.
        /// </summary>
        /// <param name="module">The query handler module.</param>
        public GetByIdQueryHandler(IQueryHandlerModule module)
            : base(module)
        {
        }

        /// <inheritdoc/>
        protected override async Task<EventModel> ProtectedHandleAsync(GetByIdQuery request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var principal = PrincipalProvider.GetPrincipal();
            if (principal == null)
            {
                throw new ForbiddenException("Not authenticated");
            }

            Expression<Func<EventEntity, bool>> expression = PredicateBuilder.Create<EventEntity>(e => e.Id == Guid.Parse(request.Id));
            if (!principal.IsInRole(RoleType.Administrator))
            {
                expression = expression.And(e => e.UserId == principal.Identity.Name);
            }

            var result = Context.EventRepository
                .GetQuery()
                .Where(expression)
                .FirstOrDefault();

            if (result == null)
            {
                throw new NotFoundException(nameof(WebhookEntity), request.Id);
            }

            var model = EventModel.Create(result, Serializer);

            await Task.CompletedTask;

            return model;
        }
    }
}
