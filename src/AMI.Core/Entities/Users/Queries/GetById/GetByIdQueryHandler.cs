using System;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Constants;
using AMI.Core.Entities.Models;
using AMI.Core.Repositories;
using AMI.Domain.Entities;
using AMI.Domain.Exceptions;
using MediatR;

namespace AMI.Core.Entities.Users.Queries.GetById
{
    /// <summary>
    /// A handler for queries to get an object by its identifier.
    /// </summary>
    public class GetByIdQueryHandler : IRequestHandler<GetByIdQuery, UserModel>
    {
        private readonly IAmiUnitOfWork context;
        private readonly IApplicationConstants constants;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetByIdQueryHandler"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="constants">The application constants.</param>
        public GetByIdQueryHandler(IAmiUnitOfWork context, IApplicationConstants constants)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.constants = constants ?? throw new ArgumentNullException(nameof(constants));
        }

        /// <inheritdoc/>
        public async Task<UserModel> Handle(GetByIdQuery request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var result = await context.UserRepository.GetFirstOrDefaultAsync(e => e.Id == Guid.Parse(request.Id), cancellationToken);

            if (result == null)
            {
                throw new NotFoundException(nameof(ObjectEntity), request.Id);
            }

            var model = UserModel.Create(result, constants);

            return model;
        }
    }
}
