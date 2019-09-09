using System;
using System.Runtime.Serialization;
using AMI.Core.Entities.Shared.Models;
using AMI.Core.IO.Converters;
using AMI.Core.Workers;
using AMI.Domain.Enums;
using Newtonsoft.Json;

namespace AMI.Core.Entities.Models
{
    /// <summary>
    /// The base all results have in common.
    /// </summary>
    [JsonConverter(typeof(JsonInheritanceConverter), "discriminator")]
    [KnownType(typeof(QueueWorkerModel))]
    [KnownType(typeof(RecurringWorkerModel))]
    public abstract class BaseWorkerModel : IEntity
    {
        /// <summary>
        /// Gets or sets the identifier of the worker.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the worker.
        /// </summary>
        public string WorkerName { get; set; }

        /// <summary>
        /// Gets or sets the type of the worker.
        /// </summary>
        public WorkerType WorkerType { get; set; }

        /// <summary>
        /// Gets or sets the current status of the worker.
        /// </summary>
        public WorkerStatus WorkerStatus { get; set; }

        /// <summary>
        /// Gets or sets the last activity date.
        /// </summary>
        public DateTime LastActivityDate { get; set; }

        /// <summary>
        /// Gets or sets the current processing time.
        /// </summary>
        public TimeSpan CurrentProcessingTime { get; set; }

        /// <summary>
        /// Gets or sets the last processing time.
        /// </summary>
        public TimeSpan LastProcessingTime { get; set; }

        /// <summary>
        /// Creates a model based on the given domain entity.
        /// </summary>
        /// <param name="entity">The domain entity.</param>
        /// <returns>The domain entity as a model.</returns>
        public static BaseWorkerModel Create(IBaseWorker entity)
        {
            if (entity == null)
            {
                return null;
            }

            BaseWorkerModel model = null;

            switch (entity)
            {
                case IQueueWorker queueEntity:
                    model = QueueWorkerModel.Create(queueEntity);
                    break;
                case IRecurringWorker recurringEntity:
                    model = RecurringWorkerModel.Create(recurringEntity);
                    break;
                default:
                    break;
            }

            SetBase(model, entity);

            return model;
        }

        private static void SetBase(BaseWorkerModel model, IBaseWorker entity)
        {
            if (entity != null && model != null)
            {
                model.Id = entity.Id.ToString();
                model.WorkerName = entity.WorkerName;
                model.WorkerType = entity.WorkerType;
                model.WorkerStatus = entity.WorkerStatus;
                model.LastActivityDate = entity.LastActivityDate;
                model.CurrentProcessingTime = entity.CurrentProcessingTime;
                model.LastProcessingTime = entity.LastProcessingTime;
            }
        }
    }
}
