using System;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Constants;
using AMI.Core.IO.Serializers;
using AMI.Core.Modules;
using AMI.Core.Providers;
using AMI.Core.Repositories;
using AMI.Domain.Exceptions;
using MediatR;
using RNS.Framework.Tools;

namespace AMI.Core.Entities.Shared.Queries
{
    /// <summary>
    /// The base handler for query requests.
    /// </summary>
    /// <typeparam name="TRequest">The type of the request.</typeparam>
    /// <typeparam name="TResponse">The type of the response.</typeparam>
    /// <seealso cref="IRequestHandler{TRequest, TResponse}" />
    public abstract class BaseQueryRequestHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseQueryRequestHandler{TRequest, TResponse}"/> class.
        /// </summary>
        /// <param name="module">The query handler module.</param>
        public BaseQueryRequestHandler(IQueryHandlerModule module)
        {
            Ensure.ArgumentNotNull(module, nameof(module));

            Context = module.Context ?? throw new UnexpectedNullException("The context cannot be null.");
            Constants = module.Constants ?? throw new UnexpectedNullException("The application constants cannot be null.");
            PrincipalProvider = module.PrincipalProvider ?? throw new UnexpectedNullException("The principal provider cannot be null.");
            Serializer = module.Serializer ?? throw new UnexpectedNullException("The JSON serializer cannot be null.");
        }

        /// <summary>
        /// Gets the context.
        /// </summary>
        protected IAmiUnitOfWork Context { get; private set; }

        /// <summary>
        /// Gets the application constants.
        /// </summary>
        protected IApplicationConstants Constants { get; private set; }

        /// <summary>
        /// Gets the principal provider.
        /// </summary>
        protected ICustomPrincipalProvider PrincipalProvider { get; private set; }

        /// <summary>
        /// Gets the JSON serializer.
        /// </summary>
        protected IDefaultJsonSerializer Serializer { get; private set; }

        /// <inheritdoc/>
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                // TODO: audit response
                var response = await ProtectedHandleAsync(request, cancellationToken);
                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Asynchronously handles the query request called by the base class.
        /// </summary>
        /// <param name="request">The query request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result contains the result of the query request.</returns>
        protected abstract Task<TResponse> ProtectedHandleAsync(TRequest request, CancellationToken cancellationToken);
    }
}
