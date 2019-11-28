using System.Threading;

namespace AMI.Core.Wrappers
{
    /// <summary>
    /// An interface representing a wrapped job cancellation token.
    /// </summary>
    public interface IWrappedJobCancellationToken
    {
        /// <summary>
        /// Gets the shutdown token.
        /// </summary>
        CancellationToken ShutdownToken { get; }

        /// <summary>
        /// Throws if cancellation is requested.
        /// </summary>
        void ThrowIfCancellationRequested();
    }
}
