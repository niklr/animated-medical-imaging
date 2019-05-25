using System;
using System.Net;
using System.Threading.Tasks;
using AMI.Core.Configuration;
using AMI.Core.Entities.ViewModels;
using AMI.Core.Serializers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PNL.Application.Exceptions;

namespace AMI.API.Handlers
{
    /// <summary>
    /// A custom exception handler.
    /// </summary>
    /// <seealso cref="ICustomExceptionHandler" />
    public class CustomExceptionHandler : ICustomExceptionHandler
    {
        private readonly IAmiConfigurationManager configuration;
        private readonly ILogger<CustomExceptionHandler> logger;
        private readonly IDefaultJsonSerializer serializer;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomExceptionHandler"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="serializer">The serializer.</param>
        /// <exception cref="ArgumentNullException">
        /// configuration
        /// or
        /// loggerFactory
        /// or
        /// serializer
        /// </exception>
        public CustomExceptionHandler(IAmiConfigurationManager configuration, ILoggerFactory loggerFactory, IDefaultJsonSerializer serializer)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            logger = loggerFactory?.CreateLogger<CustomExceptionHandler>() ?? throw new ArgumentNullException(nameof(loggerFactory));
            this.serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
        }

        /// <summary>
        /// Handles the exception.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="exception">The exception.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task HandleException(HttpContext context, Exception exception)
        {
            if (!context.Response.HasStarted)
            {
                logger.LogWarning(exception, exception.Message);

                var code = HttpStatusCode.InternalServerError;

                var result = new ErrorResult()
                {
                    Error = exception.Message,
                    StackTrace = configuration.IsDevelopment ? exception.StackTrace : string.Empty
                };

                switch (exception.GetType().Name)
                {
                    case nameof(AuthException):
                        code = HttpStatusCode.Unauthorized;
                        break;
                    case nameof(ForbiddenException):
                        code = HttpStatusCode.Forbidden;
                        break;
                    case nameof(ValidationException):
                        code = HttpStatusCode.BadRequest;
                        result.ValidationErrors = ((ValidationException)exception).Failures;
                        break;
                    case nameof(DeleteFailureException):
                    case nameof(UpdateFailureException):
                        code = HttpStatusCode.BadRequest;
                        break;
                    case nameof(NotFoundException):
                        code = HttpStatusCode.NotFound;
                        break;
                    case nameof(OutOfSyncException):
                        code = HttpStatusCode.Conflict;
                        break;
                    default:
                        break;
                }

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)code;
                await context.Response.WriteAsync(serializer.Serialize(result));
            }
        }
    }
}
