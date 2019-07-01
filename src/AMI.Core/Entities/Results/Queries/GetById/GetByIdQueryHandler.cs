using System;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Entities.Models;
using AMI.Core.IO.Serializers;
using AMI.Core.Repositories;
using AMI.Domain.Entities;
using AMI.Domain.Exceptions;
using MediatR;

namespace AMI.Core.Entities.Results.Queries.GetById
{
    /// <summary>
    /// A handler for queries to get a result by its identifier.
    /// </summary>
    public class GetByIdQueryHandler : IRequestHandler<GetByIdQuery, ResultModel>
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

        /// <summary>
        /// Handles the specified request.
        /// </summary>
        /// <param name="request">The query request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result contains the image file information.</returns>
        public async Task<ResultModel> Handle(GetByIdQuery request, CancellationToken cancellationToken)
        {
            var entity = await context.ResultRepository
                .GetFirstOrDefaultAsync(e => e.Id == Guid.Parse(request.Id), cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(ResultEntity), request.Id);
            }

            return ResultModel.Create(entity, serializer);
        }
    }
}
