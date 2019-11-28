using System;
using System.Threading.Tasks;
using AMI.Core.Constants;
using AMI.Core.Services;
using AMI.Hangfire.Attributes;
using AMI.Hangfire.Wrappers;
using Hangfire;
using RNS.Framework.Tools;

namespace AMI.Hangfire.Proxies
{
    /// <summary>
    /// A proxy to the service handling tasks related to background processing.
    /// </summary>
    [LogEverything]
    public class TaskServiceProxy
    {
        private readonly ITaskService service;

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskServiceProxy"/> class.
        /// </summary>
        /// <param name="service">The task service.</param>
        public TaskServiceProxy(ITaskService service)
        {
            this.service = service ?? throw new ArgumentNullException(nameof(service));
        }

        /// <summary>
        /// Processes the task asynchronous.
        /// </summary>
        /// <param name="id">The identifier of the task.</param>
        /// <param name="ct">The cancellation token.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Queue(QueueNames.Imaging)]
        [LogEverything]
        public async Task ProcessAsync(string id, IJobCancellationToken ct)
        {
            Ensure.ArgumentNotNullOrWhiteSpace(id, nameof(id));
            Ensure.ArgumentNotNull(ct, nameof(ct));

            await service.ProcessAsync(id, new JobCancellationTokenWrapper(ct));
        }
    }
}
