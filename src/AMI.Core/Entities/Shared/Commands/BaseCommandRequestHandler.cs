using System;
using System.Threading;
using System.Threading.Tasks;
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
        public BaseCommandRequestHandler()
        {
        }

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
