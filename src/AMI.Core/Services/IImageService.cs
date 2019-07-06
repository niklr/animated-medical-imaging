using System;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Entities.Models;
using AMI.Core.Entities.Results.Commands.ProcessPath;

namespace AMI.Core.Services
{
    /// <summary>
    /// An interface representing a service for imaging purposes.
    /// </summary>
    public interface IImageService
    {
        /// <summary>
        /// Processes images based on the provided path command information asynchronous.
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
        Task<ProcessResultModel> ProcessAsync(ProcessPathCommand command, CancellationToken ct);
    }
}
