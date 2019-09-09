﻿using System;
using System.Threading;
using AMI.Core.Services;
using Microsoft.Extensions.Logging;

namespace AMI.Core.Workers
{
    /// <summary>
    /// A recurring worker.
    /// </summary>
    /// <seealso cref="BaseWorker" />
    public abstract class RecurringWorker : BaseWorker, IRecurringWorker
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RecurringWorker"/> class.
        /// </summary>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="workerService">The worker service.</param>
        public RecurringWorker(ILoggerFactory loggerFactory, IWorkerService workerService)
            : base(loggerFactory, workerService)
        {
        }

        /// <inheritdoc/>
        public Timer Timer { get; set; }

        /// <inheritdoc/>
        public DateTime NextActivityDate { get; set; }
    }
}
