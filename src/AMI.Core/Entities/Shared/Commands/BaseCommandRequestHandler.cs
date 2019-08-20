using System;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Entities.Models;
using AMI.Core.Modules;
using AMI.Core.Providers;
using AMI.Core.Repositories;
using AMI.Core.Services;
using AMI.Domain.Enums.Auditing;
using AMI.Domain.Exceptions;
using MediatR;
using RNS.Framework.Tools;

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
        /// <param name="module">The command handler module.</param>
        public BaseCommandRequestHandler(ICommandHandlerModule module)
        {
            Ensure.ArgumentNotNull(module, nameof(module));

            Audit = module.Audit ?? throw new UnexpectedNullException("The auditing service cannot be null.");
            Context = module.Context ?? throw new UnexpectedNullException("The context cannot be null.");
            Gateway = module.Gateway ?? throw new UnexpectedNullException("The gateway service cannot be null.");
            IdentityService = module.IdentityService ?? throw new UnexpectedNullException("The identity service cannot be null.");
            PrincipalProvider = module.PrincipalProvider ?? throw new UnexpectedNullException("The principal provider cannot be null.");
        }

        /// <summary>
        /// Gets the auditing service.
        /// </summary>
        protected IAuditService Audit { get; private set; }

        /// <summary>
        /// Gets the context.
        /// </summary>
        protected IAmiUnitOfWork Context { get; private set; }

        /// <summary>
        /// Gets the gateway service.
        /// </summary>
        protected IGatewayService Gateway { get; private set; }

        /// <summary>
        /// Gets the identity service.
        /// </summary>
        protected IIdentityService IdentityService { get; private set; }

        /// <summary>
        /// Gets the principal provider.
        /// </summary>
        protected ICustomPrincipalProvider PrincipalProvider { get; private set; }

        /// <summary>
        /// Gets the type of the sub event.
        /// </summary>
        protected abstract SubEventType SubEventType { get; }

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

                var response = await ProtectedHandleAsync(request, cancellationToken);

                if (SubEventType != SubEventType.None)
                {
                    var principal = PrincipalProvider.GetPrincipal();
                    var auditData = new AuditEventDataModel()
                    {
                        Entity = response,
                        Command = request
                    };
                    await Audit.AddDefaultEventAsync(principal, auditData, SubEventType);
                }

                return response;
            }
            catch (Exception)
            {
                Context.RollBackTransaction();

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
