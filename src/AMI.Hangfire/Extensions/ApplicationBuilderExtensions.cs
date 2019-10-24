using System;
using System.Net;
using System.Threading.Tasks;
using AMI.Core.Services;
using AMI.Domain.Exceptions;
using AMI.Hangfire.Filters;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using RNS.Framework.Tools;

namespace AMI.Hangfire.Extensions
{
    /// <summary>
    /// Extensions related to <see cref="IApplicationBuilder"/>.
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Extension method used to add the dashboard UI of Hangfire.
        /// </summary>
        /// <param name="builder">The application builder.</param>
        public static void UseCustomHangfireDashboard(this IApplicationBuilder builder)
        {
            Ensure.ArgumentNotNull(builder, nameof(builder));

            string pathMatch = "/hangfire";
            string appPath = "/account/login";

            builder.UseStatusCodePages(context =>
            {
                var request = context.HttpContext.Request;
                var response = context.HttpContext.Response;

                if (request.Path.HasValue && request.Path.Value.StartsWith(pathMatch) &&
                    response.StatusCode == (int)HttpStatusCode.Unauthorized)
                {
                    response.Redirect(appPath);
                }

                return Task.CompletedTask;
            });

            var options = new DashboardOptions
            {
                Authorization = new[] { new CustomHangfireAuthorizationFilter() },
                AppPath = appPath
            };

            builder.UseHangfireDashboard(pathMatch, options);
        }

        /// <summary>
        /// Extension method used to schedule recurring Hangfire jobs.
        /// </summary>
        /// <param name="builder">The application builder.</param>
        public static void ScheduleRecurringHangfireJobs(this IApplicationBuilder builder)
        {
            Ensure.ArgumentNotNull(builder, nameof(builder));

            var serviceProvider = builder.ApplicationServices;
            if (serviceProvider == null)
            {
                throw new UnexpectedNullException($"{nameof(IServiceProvider)} could not be retrieved.");
            }

            var backgroundService = serviceProvider.GetRequiredService<IBackgroundService>();
            if (backgroundService == null)
            {
                throw new UnexpectedNullException($"{nameof(IBackgroundService)} could not be retrieved.");
            }

            backgroundService.ScheduleCleanup();
        }
    }
}
