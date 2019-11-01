using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.IO.Clients;
using AMI.Core.IO.Serializers;

namespace AMI.Infrastructure.IO.Clients
{
    /// <summary>
    /// A client for for sending HTTP requests and receiving HTTP responses
    /// from a resource identified by a URI.
    /// </summary>
    /// <remarks>
    /// It is recommended to instantiate one HttpClient for the application's lifetime and share it.
    /// Source: https://docs.microsoft.com/en-us/azure/architecture/antipatterns/improper-instantiation/
    /// </remarks>
    public class JsonHttpClient : HttpClient, IJsonHttpClient
    {
        private readonly string contentType = "application/json";
        private readonly IDefaultJsonSerializer serializer;

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonHttpClient"/> class.
        /// </summary>
        /// <param name="serializer">The JSON serializer.</param>
        public JsonHttpClient(IDefaultJsonSerializer serializer)
        {
            this.serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));

            DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));
        }

        /// <inheritdoc/>
        public Task<HttpResponseMessage> PostAsync(string requestUri, object content, CancellationToken cancellationToken)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, requestUri)
            {
                Content = new StringContent(serializer.Serialize(content), Encoding.UTF8, contentType)
            };

            return SendAsync(request, cancellationToken);
        }
    }
}
