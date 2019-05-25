using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace AMI.API.Handlers
{
    /// <summary>
    /// A custom exception handler.
    /// </summary>
    public interface ICustomExceptionHandler
    {
        /// <summary>
        /// Handles the exception.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="exception">The exception.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task HandleException(HttpContext context, Exception exception);
    }
}
