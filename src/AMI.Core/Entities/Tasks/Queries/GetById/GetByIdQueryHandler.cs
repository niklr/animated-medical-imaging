using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Entities.Models;
using AMI.Core.IO.Serializers;
using AMI.Core.Repositories;
using AMI.Domain.Entities;
using AMI.Domain.Exceptions;
using MediatR;

namespace AMI.Core.Entities.Tasks.Queries.GetById
{
    /// <summary>
    /// A handler for queries to get an object by its identifier.
    /// </summary>
    public class GetByIdQueryHandler : IRequestHandler<GetByIdQuery, TaskModel>
    {
        private readonly IAmiUnitOfWork context;
        private readonly IDefaultJsonSerializer serializer;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetByIdQueryHandler"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="serializer">The JSON serializer.</param>
        public GetByIdQueryHandler(IAmiUnitOfWork context, IDefaultJsonSerializer serializer)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
        }

        /// <inheritdoc/>
        public async Task<TaskModel> Handle(GetByIdQuery request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var result = context.TaskRepository
                .GetQuery()
                .Where(e => e.Id == Guid.Parse(request.Id))
                .Select(e => new
                {
                    Task = e,
                    e.Object,
                    e.Result
                })
                .FirstOrDefault();

            if (result == null)
            {
                throw new NotFoundException(nameof(ObjectEntity), request.Id);
            }

            var model = TaskModel.Create(result.Task, result.Object, result.Result, serializer);

            await Task.CompletedTask;

            return model;
        }
    }
}
