using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Constants;
using AMI.Core.Entities.Models;
using AMI.Core.Entities.Users.Models;
using AMI.Core.IO.Builders;
using AMI.Core.Repositories;
using AMI.Core.Services;
using AMI.Domain.Enums;

namespace AMI.Infrastructure.Services
{
    /// <summary>
    /// The gateway service.
    /// </summary>
    public class GatewayService : IGatewayService
    {
        private readonly IAmiUnitOfWork context;
        private readonly IApplicationConstants constants;
        private readonly IGatewayObserverService gatewayObserverService;

        /// <summary>
        /// Initializes a new instance of the <see cref="GatewayService"/> class.
        /// </summary>
        /// <param name="builder">The builder for gateway group names.</param>
        /// <param name="context">The context.</param>
        /// <param name="constants">The application constants.</param>
        /// <param name="gatewayObserverService">The gateway observer service.</param>
        public GatewayService(
            IGatewayGroupNameBuilder builder,
            IAmiUnitOfWork context,
            IApplicationConstants constants,
            IGatewayObserverService gatewayObserverService)
        {
            Builder = builder ?? throw new ArgumentNullException(nameof(builder));
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.constants = constants ?? throw new ArgumentNullException(nameof(constants));
            this.gatewayObserverService = gatewayObserverService ?? throw new ArgumentNullException(nameof(gatewayObserverService));
        }

        /// <inheritdoc/>
        public IGatewayGroupNameBuilder Builder { get; }

        /// <inheritdoc/>
        public IEnumerable<string> GetGroupNames(string userId)
        {
            var groupNames = new List<string>();

            if (!string.IsNullOrWhiteSpace(userId))
            {
                groupNames.Add(Builder.BuildUserIdGroupName(userId));

                var principal = GetPrincipal(userId);
                if (principal != null)
                {
                    if (principal.IsInRole(RoleType.Administrator))
                    {
                        groupNames.Add(Builder.BuildAdministratorGroupName());
                    }
                }
            }

            return groupNames;
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

        /// <inheritdoc/>
        public async Task NotifyGroupsAsync<T>(string userId, GatewayOpCode gatewayOpCode, GatewayEvent gatewayEvent, T data, CancellationToken ct)
        {
            if (!string.IsNullOrWhiteSpace(userId))
            {
                await NotifyGroupAsync(Builder.BuildUserIdGroupName(userId), gatewayOpCode, gatewayEvent, data, ct);
            }

            await NotifyGroupAsync(Builder.BuildAdministratorGroupName(), gatewayOpCode, gatewayEvent, data, ct);
        }

        private ICustomPrincipal GetPrincipal(string userId)
        {
            if (Guid.TryParse(userId, out Guid parsedUserId))
            {
                var result = context.UserRepository.GetFirstOrDefault(e => e.Id == parsedUserId);
                if (result != null)
                {
                    return new EntityPrincipal(result, constants);
                }
            }

            return null;
        }
    }
}
