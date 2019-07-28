using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Entities.Models;
using AMI.Core.IO.Builders;
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
        /// <param name="builder">The builder for gateway group names.</param>
        /// <param name="gatewayObserverService">The gateway observer service.</param>
        /// <exception cref="ArgumentNullException">gatewayObserverService</exception>
        public GatewayService(IGatewayGroupNameBuilder builder, IGatewayObserverService gatewayObserverService)
        {
            Builder = builder ?? throw new ArgumentNullException(nameof(builder));
            this.gatewayObserverService = gatewayObserverService ?? throw new ArgumentNullException(nameof(gatewayObserverService));
        }

        /// <inheritdoc/>
        public IGatewayGroupNameBuilder Builder { get; }

        /// <inheritdoc/>
        public IEnumerable<string> GetGroupNames(string userId)
        {
            // TODO: replace default group with individual groups per user
            return new List<string>() { Builder.BuildDefaultGroupName() };
        }

        /// <inheritdoc/>
        public Task NotifyGroupAsync<T>(string groupName, GatewayOpCode gatewayOpCode, GatewayEvent gatewayEvent, T data, CancellationToken ct)
        {
            var result = new GatewayResultModel<T>()
            {
                d = data,
                op = gatewayOpCode,
                t = gatewayEvent
            };

            return gatewayObserverService.NotifyAsync(groupName, result, ct);
        }
    }
}
