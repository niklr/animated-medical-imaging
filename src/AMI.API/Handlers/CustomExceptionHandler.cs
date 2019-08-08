using System;
using System.Net;
using System.Threading.Tasks;
using AMI.Core.Configurations;
using AMI.Core.Entities.Models;
using AMI.Core.IO.Serializers;
using AMI.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace AMI.API.Handlers
{
    /// <summary>
    /// A custom exception handler.
    /// </summary>
    /// <seealso cref="ICustomExceptionHandler" />
    public class CustomExceptionHandler : ICustomExceptionHandler
    {
        private readonly IApiConfiguration configuration;
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
        public CustomExceptionHandler(IApiConfiguration configuration, ILoggerFactory loggerFactory, IDefaultJsonSerializer serializer)
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
                var code = HttpStatusCode.InternalServerError;

                var result = new ErrorModel()
                {
                    Error = exception.Message,
                    StackTrace = configuration.Options.IsDevelopment ? exception.StackTrace : string.Empty
                };

                switch (exception.GetType().Name)
                {
                    case nameof(AuthException):
                        logger.LogInformation(exception, exception.Message);
                        code = HttpStatusCode.Unauthorized;
                        break;
                    case nameof(ForbiddenException):
                        logger.LogInformation(exception, exception.Message);
                        code = HttpStatusCode.Forbidden;
                        break;
                    case nameof(ArgumentException):
                    case nameof(ArgumentNullException):
                        logger.LogInformation(exception, exception.Message);
                        code = HttpStatusCode.BadRequest;
                        break;
                    case nameof(ValidationException):
                        logger.LogInformation(exception, exception.Message);
                        code = HttpStatusCode.BadRequest;
                        result.ValidationErrors = ((ValidationException)exception).Failures;
                        break;
                    case nameof(DeleteFailureException):
                    case nameof(UpdateFailureException):
                        logger.LogInformation(exception, exception.Message);
                        code = HttpStatusCode.BadRequest;
                        break;
                    case nameof(NotFoundException):
                        logger.LogInformation(exception, exception.Message);
                        code = HttpStatusCode.NotFound;
                        break;
                    case nameof(OutOfSyncException):
                        logger.LogInformation(exception, exception.Message);
                        code = HttpStatusCode.Conflict;
                        break;
                    default:
                        logger.LogWarning(exception, exception.Message);
                        break;
                }

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)code;
                await context.Response.WriteAsync(serializer.Serialize(result));
            }
        }
    }
}
