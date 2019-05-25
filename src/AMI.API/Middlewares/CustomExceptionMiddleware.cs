using System;
using System.Threading.Tasks;
using AMI.API.Handlers;
using Microsoft.AspNetCore.Http;

namespace AMI.API.Middlewares
{
    /// <summary>
    /// A custom middleware to handle exceptions.
    /// </summary>
    public class CustomExceptionMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ICustomExceptionHandler handler;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomExceptionMiddleware"/> class.
        /// </summary>
        /// <param name="next">The request delegate.</param>
        /// <param name="handler">The exception handler.</param>
        /// <exception cref="ArgumentNullException">
        /// next
        /// or
        /// handler
        /// </exception>
        public CustomExceptionMiddleware(RequestDelegate next, ICustomExceptionHandler handler)
        {
            this.next = next ?? throw new ArgumentNullException(nameof(next));
            this.handler = handler ?? throw new ArgumentNullException(nameof(handler));
        }

        /// <summary>
        /// Invokes the request delegate on the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception e)
            {
                await handler.HandleException(context, e);
            }
        }
    }
}
