using System.Threading;
using AMI.Core.Configurations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace AMI.API.Controllers
{
    /// <summary>
    /// Base controller
    /// </summary>
    [ApiController]
    public abstract class BaseController : Controller
    {
        private IMediator mediator;

        /// <summary>
        /// Gets the mediator.
        /// </summary>
        protected IMediator Mediator => mediator ?? (mediator = HttpContext.RequestServices.GetService<IMediator>());

        /// <summary>
        /// Gets the cancellation token.
        /// </summary>
        protected CancellationToken CancellationToken
        {
            get
            {
                IAppConfiguration configuration = HttpContext.RequestServices.GetService<IAppConfiguration>();
                CancellationTokenSource cts = CancellationTokenSource.CreateLinkedTokenSource(
                    HttpContext?.RequestAborted ?? default(CancellationToken));

                if (configuration?.Options?.TimeoutMilliseconds > 0)
                {
                    cts.CancelAfter(configuration.Options.TimeoutMilliseconds);
                }

                return cts.Token;
            }
        }
    }
}
