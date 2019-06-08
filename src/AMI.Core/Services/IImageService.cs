using System;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Entities.Models;
using AMI.Core.Entities.Shared.Commands;

namespace AMI.Core.Services
{
    /// <summary>
    /// A service for imaging purposes.
    /// </summary>
    public interface IImageService
    {
        /// <summary>
        /// Processes images based on the provided command information asynchronous.
        /// </summary>
        /// <param name="command">The command information.</param>
        /// <param name="ct">The cancellation token.</param>
        /// <returns>
        /// The result information.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// command
        /// or
        /// ct
        /// </exception>
        /// <exception cref="NotSupportedException">Process command type is not supported.</exception>
        Task<ProcessResultModel> ProcessAsync(BaseProcessCommand command, CancellationToken ct);
    }
}
