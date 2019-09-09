using AMI.Core.Workers;

namespace AMI.Core.Entities.Models
{
    /// <summary>
    /// A model containing information about the default worker.
    /// </summary>
    public class QueueWorkerModel : BaseWorkerModel
    {
        /// <summary>
        /// Gets or sets the queue count.
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Creates a model based on the given domain entity.
        /// </summary>
        /// <param name="entity">The domain entity.</param>
        /// <returns>The domain entity as a model.</returns>
        public static QueueWorkerModel Create(IQueueWorker entity)
        {
            if (entity == null)
            {
                return null;
            }

            var model = new QueueWorkerModel
            {
                Count = entity.Count
            };

            return model;
        }
    }
}
