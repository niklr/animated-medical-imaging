using System;
using System.Threading.Tasks;
using AMI.API.Handlers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AMI.API.Attributes
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
        /// <param name="handler">The exception handler.</param>
        /// <exception cref="ArgumentNullException">handler</exception>
        public CustomExceptionFilterAttribute(ICustomExceptionHandler handler)
            : base()
        {
            this.handler = handler ?? throw new ArgumentNullException(nameof(handler));
        }

        /// <inheritdoc/>
        public override async Task OnExceptionAsync(ExceptionContext context)
        {
            await handler.HandleException(context.HttpContext, context.Exception);
        }
    }
}
