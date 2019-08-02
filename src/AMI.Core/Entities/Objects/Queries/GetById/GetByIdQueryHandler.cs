using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Entities.Models;
using AMI.Core.Entities.Shared.Queries;
using AMI.Core.Modules;
using AMI.Domain.Entities;
using AMI.Domain.Exceptions;

namespace AMI.Core.Entities.Objects.Queries.GetById
{
    /// <summary>
    /// A query handler to get an object by its identifier.
    /// </summary>
    public class GetByIdQueryHandler : BaseQueryRequestHandler<GetByIdQuery, ObjectModel>
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
        protected override async Task<ObjectModel> ProtectedHandleAsync(GetByIdQuery request, CancellationToken cancellationToken)
        {
            var result = Context.ObjectRepository
                .GetQuery()
                .Where(e => e.Id == Guid.Parse(request.Id))
                .Select(e => new
                {
                    Object = e,
                    Tasks = e.Tasks.OrderByDescending(e1 => e1.CreatedDate).Take(Constants.DefaultPaginationLimit),
                    Results = e.Tasks.OrderByDescending(e1 => e1.CreatedDate).Select(e1 => e1.Result).Take(Constants.DefaultPaginationLimit)
                })
                .FirstOrDefault();

            if (result == null)
            {
                throw new NotFoundException(nameof(ObjectEntity), request.Id);
            }

            var model = ObjectModel.Create(result.Object, result.Tasks, result.Results, Serializer);

            await Task.CompletedTask;

            return model;
        }
    }
}
