using System;
using System.Threading.Tasks;
using AMI.Core.Services;
using AMI.Hangfire.Wrappers;
using Hangfire;
using RNS.Framework.Tools;

namespace AMI.Hangfire.Proxies
{
    /// <summary>
    /// A proxy to the service scheduling cleanups.
    /// </summary>
    public class CleanupServiceProxy
    {
        private readonly ICleanupService service;

        /// <summary>
        /// Initializes a new instance of the <see cref="CleanupServiceProxy"/> class.
        /// </summary>
        /// <param name="service">The service.</param>
        public CleanupServiceProxy(ICleanupService service)
        {
            this.service = service ?? throw new ArgumentNullException(nameof(service));
        }

        /// <summary>
        /// Calls the cleanup asynchronous.
        /// </summary>
        /// <param name="ct">The cancellation token.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task CleanupAsync(IJobCancellationToken ct)
        {
            Ensure.ArgumentNotNull(ct, nameof(ct));

            await service.CleanupAsync(new JobCancellationTokenWrapper(ct));
        }
    }
}
