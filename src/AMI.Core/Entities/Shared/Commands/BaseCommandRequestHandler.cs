using System;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Providers;
using AMI.Core.Repositories;
using AMI.Core.Services;
using MediatR;

namespace AMI.Core.Entities.Shared.Commands
{
    /// <summary>
    /// The base handler for command requests.
    /// </summary>
    /// <typeparam name="TRequest">The type of the request.</typeparam>
    /// <typeparam name="TResponse">The type of the response.</typeparam>
    /// <seealso cref="IRequestHandler{TRequest, TResponse}" />
    public abstract class BaseCommandRequestHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseCommandRequestHandler{TRequest, TResponse}"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="gateway">The gateway service.</param>
        /// <param name="principalProvider">The principal provider.</param>
        public BaseCommandRequestHandler(IAmiUnitOfWork context, IGatewayService gateway, ICustomPrincipalProvider principalProvider)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
            Gateway = gateway ?? throw new ArgumentNullException(nameof(gateway));
            PrincipalProvider = principalProvider ?? throw new ArgumentNullException(nameof(principalProvider));
        }

        /// <summary>
        /// Gets the context.
        /// </summary>
        protected IAmiUnitOfWork Context { get; private set; }

        /// <summary>
        /// Gets the gateway service.
        /// </summary>
        protected IGatewayService Gateway { get; private set; }

        /// <summary>
        /// Gets the principal provider.
        /// </summary>
        protected ICustomPrincipalProvider PrincipalProvider { get; private set; }

        /// <summary>
        /// Asynchronously handles the command request.
        /// </summary>
        /// <param name="request">The command request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result contains the result of the command request.</returns>
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
        /// Asynchronously handles the command request called by the base class.
        /// </summary>
        /// <param name="request">The command request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result contains the result of the command request.</returns>
        protected abstract Task<TResponse> ProtectedHandleAsync(TRequest request, CancellationToken cancellationToken);
    }
}
