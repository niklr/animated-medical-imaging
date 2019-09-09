using System;
using AMI.Core.Workers;

namespace AMI.Core.Entities.Models
{
    /// <summary>
    /// A model containing information about the recurring worker.
    /// </summary>
    public class RecurringWorkerModel : BaseWorkerModel
    {
        /// <summary>
        /// Gets or sets the next activity date.
        /// </summary>
        public DateTime NextActivityDate { get; set; }

        /// <summary>
        /// Creates a model based on the given domain entity.
        /// </summary>
        /// <param name="entity">The domain entity.</param>
        /// <returns>The domain entity as a model.</returns>
        public static RecurringWorkerModel Create(IRecurringWorker entity)
        {
            if (entity == null)
            {
                return null;
            }

            var model = new RecurringWorkerModel
            {
                NextActivityDate = entity.NextActivityDate
            };

            return model;
        }
    }
}
