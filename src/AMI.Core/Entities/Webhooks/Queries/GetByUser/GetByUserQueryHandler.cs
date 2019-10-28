using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Entities.Models;
using AMI.Core.Entities.Shared.Queries;
using AMI.Core.Extensions.StringExtensions;
using AMI.Core.Modules;
using AMI.Domain.Entities;
using AMI.Domain.Enums;
using RNS.Framework.Search;

namespace AMI.Core.Entities.Webhooks.Queries.GetByUser
{
    /// <summary>
    /// A query handler to get webhooks of a user.
    /// </summary>
    public class GetByUserQueryHandler : BaseQueryRequestHandler<GetByUserQuery, IEnumerable<WebhookModel>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetByUserQueryHandler"/> class.
        /// </summary>
        /// <param name="module">The query handler module.</param>
        public GetByUserQueryHandler(IQueryHandlerModule module)
            : base(module)
        {
        }

        /// <inheritdoc/>
        protected override async Task<IEnumerable<WebhookModel>> ProtectedHandleAsync(GetByUserQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<WebhookEntity, bool>> expression = PredicateBuilder.Create<WebhookEntity>(e => e.UserId == request.UserId);
            if (request.EventType != EventType.Unknown)
            {
                string embeddedWildcard = Constants.WildcardCharacter.Embed(Constants.ValueSeparator);
                string embeddedEvent = request.EventType.ToString().Embed(Constants.ValueSeparator);
                expression = expression.And(e => e.EnabledEvents.Contains(embeddedWildcard) || e.EnabledEvents.Contains(embeddedEvent));
            }

            var query = Context.WebhookRepository
                .GetQuery(expression)
                .OrderByDescending(e => e.CreatedDate);

            var entities = await Context.ToListAsync(query, cancellationToken);
            var result = entities.Select(e => WebhookModel.Create(e, Constants));

            return result;
        }
    }
}
