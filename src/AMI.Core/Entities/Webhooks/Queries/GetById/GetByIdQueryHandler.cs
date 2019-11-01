using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Constants;
using AMI.Core.Entities.Models;
using AMI.Core.Entities.Shared.Queries;
using AMI.Core.Modules;
using AMI.Domain.Entities;
using AMI.Domain.Enums;
using AMI.Domain.Exceptions;
using RNS.Framework.Search;

namespace AMI.Core.Entities.Webhooks.Queries.GetById
{
    /// <summary>
    /// A query handler to get an entity by its identifier.
    /// </summary>
    public class GetByIdQueryHandler : BaseQueryRequestHandler<GetByIdQuery, WebhookModel>
    {
        private readonly IApplicationConstants constants;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetByIdQueryHandler"/> class.
        /// </summary>
        /// <param name="module">The query handler module.</param>
        /// <param name="constants">The application constants.</param>
        public GetByIdQueryHandler(IQueryHandlerModule module, IApplicationConstants constants)
            : base(module)
        {
            this.constants = constants ?? throw new ArgumentNullException(nameof(constants));
        }

        /// <inheritdoc/>
        protected override async Task<WebhookModel> ProtectedHandleAsync(GetByIdQuery request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var principal = PrincipalProvider.GetPrincipal();
            if (principal == null)
            {
                throw new ForbiddenException("Not authenticated");
            }

            Expression<Func<WebhookEntity, bool>> expression = PredicateBuilder.Create<WebhookEntity>(e => e.Id == Guid.Parse(request.Id));
            if (!principal.IsInRole(RoleType.Administrator))
            {
                expression = expression.And(e => e.UserId == principal.Identity.Name);
            }

            var result = Context.WebhookRepository
                .GetQuery()
                .Where(expression)
                .FirstOrDefault();

            if (result == null)
            {
                throw new NotFoundException(nameof(WebhookEntity), request.Id);
            }

            var model = WebhookModel.Create(result, constants);

            await Task.CompletedTask;

            return model;
        }
    }
}
