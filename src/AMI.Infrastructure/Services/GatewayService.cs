using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Entities.Models;
using AMI.Core.Services;
using AMI.Domain.Enums;

namespace AMI.Infrastructure.Services
{
    /// <summary>
    /// The gateway service.
    /// </summary>
    public class GatewayService : IGatewayService
    {
        private readonly IGatewayObserverService gatewayObserverService;

        /// <summary>
        /// Initializes a new instance of the <see cref="GatewayService"/> class.
        /// </summary>
        /// <param name="gatewayObserverService">The gateway observer service.</param>
        /// <exception cref="ArgumentNullException">gatewayObserverService</exception>
        public GatewayService(IGatewayObserverService gatewayObserverService)
        {
            this.gatewayObserverService = gatewayObserverService ?? throw new ArgumentNullException(nameof(gatewayObserverService));
        }

        /// <inheritdoc/>
        public IEnumerable<string> GetGroupNames(string userId)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task NotifyGroupAsync<T>(string groupName, GatewayOpCode gatewayOpCode, GatewayEvent gatewayEvent, T data, CancellationToken ct)
        {
            var result = new GatewayResultModel<T>()
            {
                d = data,
                op = (int)gatewayOpCode,
                t = gatewayEvent.ToString()
            };

            return gatewayObserverService.NotifyAsync(groupName, result, ct);
        }
    }
}
