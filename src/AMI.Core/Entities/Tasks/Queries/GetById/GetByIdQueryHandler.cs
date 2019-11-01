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

namespace AMI.Core.Entities.Tasks.Queries.GetById
{
    /// <summary>
    /// A query handler to get an entity by its identifier.
    /// </summary>
    public class GetByIdQueryHandler : BaseQueryRequestHandler<GetByIdQuery, TaskModel>
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
        protected override async Task<TaskModel> ProtectedHandleAsync(GetByIdQuery request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var principal = PrincipalProvider.GetPrincipal();
            if (principal == null)
            {
                throw new ForbiddenException("Not authenticated");
            }

            Expression<Func<TaskEntity, bool>> expression = PredicateBuilder.Create<TaskEntity>(e => e.Id == Guid.Parse(request.Id));
            if (!principal.IsInRole(RoleType.Administrator))
            {
                expression = expression.And(e => e.Object.UserId == principal.Identity.Name);
            }

            var result = Context.TaskRepository
                .GetQuery()
                .Where(expression)
                .Select(e => new
                {
                    Task = e,
                    e.Object,
                    e.Result
                })
                .FirstOrDefault();

            if (result == null)
            {
                throw new NotFoundException(nameof(TaskEntity), request.Id);
            }

            var model = TaskModel.Create(result.Task, result.Object, result.Result, Serializer);

            await Task.CompletedTask;

            return model;
        }
    }
}
