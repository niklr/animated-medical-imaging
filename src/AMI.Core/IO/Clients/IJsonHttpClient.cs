using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace AMI.Core.IO.Clients
{
    /// <summary>
    /// A client for for sending HTTP requests and receiving HTTP responses 
    /// from a resource identified by a URI.
    /// </summary>
    public interface IJsonHttpClient
    {
        /// <summary>
        /// Send a POST request with a cancellation token as an asynchronous operation.
        /// </summary>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="content">The HTTP request content sent to the server.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<HttpResponseMessage> PostAsync(string requestUri, object content, CancellationToken cancellationToken);
    }
}
