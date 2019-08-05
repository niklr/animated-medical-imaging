using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using RNS.Framework.Tools;

namespace AMI.Core.Behaviors
{
    /// <summary>
    /// A mediator pipeline behavior to log the performance of requests.
    /// </summary>
    /// <typeparam name="TRequest">The type of the request.</typeparam>
    /// <typeparam name="TResponse">The type of the response.</typeparam>
    /// <seealso cref="IPipelineBehavior{TRequest, TResponse}" />
    public class RequestPerformanceBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly Stopwatch timer;
        private readonly ILogger<TRequest> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestPerformanceBehavior{TRequest, TResponse}" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <exception cref="ArgumentNullException">logger</exception>
        public RequestPerformanceBehavior(ILogger<TRequest> logger)
        {
            timer = new Stopwatch();

            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc/>
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            timer.Start();

            Ensure.ArgumentNotNull(request, nameof(request));
            Ensure.ArgumentNotNull(cancellationToken, nameof(cancellationToken));
            Ensure.ArgumentNotNull(next, nameof(next));

            var response = await next();

            timer.Stop();

            if (timer.ElapsedMilliseconds > 500)
            {
                var name = typeof(TRequest).Name;

                // TODO: Add more details
                logger.LogInformation("Long Running Request: {Name} ({ElapsedMilliseconds} milliseconds) {@Request}", name, timer.ElapsedMilliseconds, request);
            }

            return response;
        }
    }
}
