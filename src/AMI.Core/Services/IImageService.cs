using System;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Entities.Models;
using AMI.Core.Entities.Objects.Commands.Process;
using AMI.Domain.Exceptions;

namespace AMI.Core.Services
{
    /// <summary>
    /// A service for imaging purposes.
    /// </summary>
    public interface IImageService
    {
        /// <summary>
        /// Processes an object based on the provided command information asynchronous.
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
        /// <exception cref="UnexpectedNullException">
        /// Empty source path.
        /// or
        /// Empty destination path.
        /// </exception>
        Task<ProcessResultModel> ProcessAsync(ProcessObjectCommand command, CancellationToken ct);
    }
}
