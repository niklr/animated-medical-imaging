using System.Threading.Tasks;
using AMI.Core.Entities.Models;
using AMI.Domain.Enums.Auditing;

namespace AMI.Core.Services
{
    /// <summary>
    /// An interface representing a service for auditing purposes.
    /// </summary>
    public interface IAuditService
    {
        /// <summary>
        /// Adds the default event asynchronous.
        /// </summary>
        /// <param name="principal">The principal.</param>
        /// <param name="data">The data.</param>
        /// <param name="subEventType">The type of the sub event.</param>
        /// <param name="outcomeType">The type of the outcome.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task AddDefaultEventAsync(
            ICustomPrincipal principal,
            object data,
            SubEventType subEventType,
            OutcomeType outcomeType = OutcomeType.Success);
    }
}
