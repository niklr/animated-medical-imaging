using System;
using System.Threading;
using AMI.Core.Wrappers;
using Hangfire;

namespace AMI.Hangfire.Wrappers
{
    /// <summary>
    /// A wrapper for job cancellation tokens.
    /// </summary>
    /// <seealso cref="IWrappedJobCancellationToken" />
    public class JobCancellationTokenWrapper : IWrappedJobCancellationToken
    {
        private readonly IJobCancellationToken token;

        /// <summary>
        /// Initializes a new instance of the <see cref="JobCancellationTokenWrapper"/> class.
        /// </summary>
        /// <param name="token">The cancellation token.</param>
        public JobCancellationTokenWrapper(IJobCancellationToken token)
        {
            this.token = token ?? throw new ArgumentNullException(nameof(token));
        }

        /// <inheritdoc/>
        public CancellationToken ShutdownToken => token.ShutdownToken;

        /// <inheritdoc/>
        public void ThrowIfCancellationRequested()
        {
            token.ThrowIfCancellationRequested();
        }
    }
}
