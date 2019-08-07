using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AMI.Domain.Entities;

namespace AMI.Core.IO.Readers
{
    /// <summary>
    /// A reader for application logs.
    /// </summary>
    public interface IAppLogReader
    {
        /// <summary>
        /// Reads the application logs.
        /// </summary>
        /// <param name="ct">The cancellation token.</param>
        /// <returns>A list of application logs.</returns>
        Task<IList<AppLogEntity>> ReadAsync(CancellationToken ct);
    }
}
