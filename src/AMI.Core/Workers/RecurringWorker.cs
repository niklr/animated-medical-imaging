using System;
using System.Threading;
using Microsoft.Extensions.Logging;

namespace AMI.Core.Workers
{
    /// <summary>
    /// A recurring worker.
    /// </summary>
    /// <seealso cref="BaseWorker" />
    public abstract class RecurringWorker : BaseWorker, IRecurringWorker
    {
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="RecurringWorker"/> class.
        /// </summary>
        /// <param name="loggerFactory">The logger factory.</param>
        public RecurringWorker(ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            logger = loggerFactory?.CreateLogger<RecurringWorker>() ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        /// <inheritdoc/>
        public Timer Timer { get; set; }

        /// <inheritdoc/>
        public DateTime NextActivityDate { get; set; }
    }
}
