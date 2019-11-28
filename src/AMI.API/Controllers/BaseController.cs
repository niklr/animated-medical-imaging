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
        private IApiConfiguration apiConfiguration;
        private IMediator mediator;

        /// <summary>
        /// Gets the API configuration.
        /// </summary>
        protected IApiConfiguration ApiConfiguration => apiConfiguration ??
            (apiConfiguration = HttpContext.RequestServices.GetService<IApiConfiguration>());

        /// <summary>
        /// Gets the application base URL.
        /// </summary>
        protected string AppBaseUrl => $"{Request.Scheme}://{Request.Host}{Request.PathBase}";

        /// <summary>
        /// Gets the mediator.
        /// </summary>
        protected IMediator Mediator => mediator ??
            (mediator = HttpContext.RequestServices.GetService<IMediator>());

        /// <summary>
        /// Gets the cancellation token.
        /// </summary>
        protected CancellationToken CancellationToken
        {
            get
            {
                CancellationTokenSource cts = CancellationTokenSource.CreateLinkedTokenSource(
                    HttpContext?.RequestAborted ?? default(CancellationToken));

                if (ApiConfiguration?.Options?.RequestTimeoutMilliseconds > 0)
                {
                    cts.CancelAfter(ApiConfiguration.Options.RequestTimeoutMilliseconds);
                }

                return cts.Token;
            }
        }
    }
}
