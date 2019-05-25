using System;
using System.Threading.Tasks;
using AMI.API.Handlers;
using AMI.Core.Configuration;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AMI.API.Filters
{
    /// <summary>
    /// CustomExceptionFilterAttribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class CustomExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly ICustomExceptionHandler handler;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomExceptionFilterAttribute"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="handler">The exception handler.</param>
        /// <exception cref="ArgumentNullException">handler</exception>
        public CustomExceptionFilterAttribute(IAmiConfigurationManager configuration, ICustomExceptionHandler handler)
            : base()
        {
            this.handler = handler ?? throw new ArgumentNullException(nameof(handler));
        }

        /// <summary>
        /// OnException
        /// </summary>
        /// <param name="context">ExceptionContext</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public override async Task OnExceptionAsync(ExceptionContext context)
        {
            await handler.HandleException(context.HttpContext, context.Exception);
        }
    }
}
