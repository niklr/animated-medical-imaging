using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Constants;
using AMI.Core.Entities.Models;
using AMI.Core.IO.Serializers;
using AMI.Core.Repositories;
using AMI.Domain.Entities;
using AMI.Domain.Exceptions;
using MediatR;

namespace AMI.Core.Entities.Objects.Queries.GetById
{
    /// <summary>
    /// A handler for queries to get an object by its identifier.
    /// </summary>
    public class GetByIdQueryHandler : IRequestHandler<GetByIdQuery, ObjectModel>
    {
        private readonly IAmiUnitOfWork uow;
        private readonly IApplicationConstants constants;
        private readonly IDefaultJsonSerializer serializer;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetByIdQueryHandler"/> class.
        /// </summary>
        /// <param name="uow">The context.</param>
        /// <param name="constants">The application constants.</param>
        /// <param name="serializer">The JSON serializer.</param>
        public GetByIdQueryHandler(IAmiUnitOfWork uow, IApplicationConstants constants, IDefaultJsonSerializer serializer)
        {
            this.uow = uow ?? throw new ArgumentNullException(nameof(uow));
            this.constants = constants ?? throw new ArgumentNullException(nameof(constants));
            this.serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
        }

        /// <inheritdoc/>
        public async Task<ObjectModel> Handle(GetByIdQuery request, CancellationToken cancellationToken)
        {
            var result = uow.ObjectRepository
                .GetQuery()
                .Where(e => e.Id == Guid.Parse(request.Id))
                .Select(e => new
                {
                    Object = e,
                    Tasks = e.Tasks.OrderByDescending(e1 => e1.CreatedDate).Take(constants.DefaultPaginationLimit),
                    Results = e.Tasks.OrderByDescending(e1 => e1.CreatedDate).Select(e1 => e1.Result).Take(constants.DefaultPaginationLimit)
                })
                .FirstOrDefault();

            if (result == null)
            {
                throw new NotFoundException(nameof(ObjectEntity), request.Id);
            }

            var model = ObjectModel.Create(result.Object, result.Tasks, result.Results, serializer);

            return await Task.Run(() => { return model; }, cancellationToken);
        }
    }
}
