using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AMI.Core.Entities.AuditEvents.Commands.Create;
using AMI.Core.Entities.Models;
using AMI.Core.Services;
using AMI.Domain.Attributes;
using AMI.Domain.Enums;
using AMI.Domain.Enums.Auditing;
using MediatR;
using Microsoft.Extensions.Logging;
using RNS.Framework.Extensions.Enum;
using RNS.Framework.Networking;
using RNS.Framework.Tools;
using XDASv2Net.Attributes;
using XDASv2Net.Model;

namespace AMI.Infrastructure.Services
{
    /// <summary>
    /// The auditing service.
    /// </summary>
    /// <seealso cref="IAuditService" />
    public class AuditService : IAuditService
    {
        private readonly ILogger logger;
        private readonly IMediator mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuditService"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="mediator">The mediator.</param>
        public AuditService(ILogger<AuditService> logger, IMediator mediator)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        /// <inheritdoc/>
        public async Task AddDefaultEventAsync(ICustomPrincipal principal, object data, SubEventType subEventType, OutcomeType outcomeType = OutcomeType.Success)
        {
            try
            {
                Ensure.ArgumentNotNull(principal, nameof(principal));

                if (!principal.IsInRole(RoleType.Service))
                {
                    XDASv2Event xdasEvent = CreateXDASEvent(principal, subEventType, outcomeType);

                    Target target = new Target()
                    {
                        Entity = CreateTargetEntity()
                    };

                    target.Data = data;

                    xdasEvent.Target = target;

                    var command = new CreateAuditEventCommand()
                    {
                        Event = xdasEvent
                    };

                    await mediator.Send(command);
                }
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
            }
        }

        private XDASv2Event CreateXDASEvent(ICustomPrincipal principal, SubEventType subEventType, OutcomeType outcomeType)
        {
            return new XDASv2Event()
            {
                Initiator = CreateInitiator(principal),
                Observer = CreateObserver(),
                Action = CreateAction(subEventType, outcomeType)
            };
        }

        private Initiator CreateInitiator(ICustomPrincipal principal)
        {
            Initiator initiator = new Initiator();
            if (principal != null && principal.Identity != null)
            {
                initiator.Account = new Account()
                {
                    Name = principal.Identity.Name,
                    Domain = principal.Identity.Domain
                };
                initiator.Assertions = new List<object>()
                {
                    new
                    {
                        principal.Identity.Name,
                        principal.Identity.Domain,
                        principal.Identity.Username,
                        principal.IpAddress
                    }
                };
            }

            return initiator;
        }

        private Observer CreateObserver()
        {
            Observer observer = new Observer()
            {
                Entity = new Entity()
                {
                    SysName = Network.GetFQDN()
                }
            };

            IPAddress ipAddress = Network.GetLocalIPAddress();
            if (ipAddress != null)
            {
                observer.Entity.SysAddr = ipAddress.ToString();
            }

            return observer;
        }

        private Entity CreateTargetEntity()
        {
            Entity entity = new Entity()
            {
                SysName = Network.GetFQDN()
            };

            IPAddress ipAddress = Network.GetLocalIPAddress();
            if (ipAddress != null)
            {
                entity.SysAddr = ipAddress.ToString();
            }

            return entity;
        }

        private XDASv2Net.Model.Action CreateAction(SubEventType subEventType, OutcomeType outcomeType)
        {
            XDASv2Net.Model.Action action = new XDASv2Net.Model.Action()
            {
                Event = new Event(),
                SubEvent = new SubEvent()
                {
                    Name = subEventType.ToString()
                },
                Time = new Time()
                {
                    Offset = (int)new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds()
                },
                Outcome = outcomeType.ToString()
            };

            BaseEventType baseEventType = subEventType.GetAttribute<EventTypeAttribute>().BaseEventType;

            switch (baseEventType)
            {
                case BaseEventType.Create:
                    action.Event.Id = XDASv2Net.Model.EventType.CREATE_DATA_ITEM.GetAttribute<EventInformationAttribute>().EventIdentifier;
                    action.Event.Name = XDASv2Net.Model.EventType.CREATE_DATA_ITEM.ToString();
                    break;
                case BaseEventType.Update:
                    action.Event.Id = XDASv2Net.Model.EventType.MODIFY_DATA_ITEM_ATTRIBUTE.GetAttribute<EventInformationAttribute>().EventIdentifier;
                    action.Event.Name = XDASv2Net.Model.EventType.MODIFY_DATA_ITEM_ATTRIBUTE.ToString();
                    break;
                case BaseEventType.Delete:
                    action.Event.Id = XDASv2Net.Model.EventType.DELETE_DATA_ITEM.GetAttribute<EventInformationAttribute>().EventIdentifier;
                    action.Event.Name = XDASv2Net.Model.EventType.DELETE_DATA_ITEM.ToString();
                    break;
                case BaseEventType.Read:
                default:
                    action.Event.Id = XDASv2Net.Model.EventType.QUERY_DATA_ITEM_ATTRIBUTE.GetAttribute<EventInformationAttribute>().EventIdentifier;
                    action.Event.Name = XDASv2Net.Model.EventType.QUERY_DATA_ITEM_ATTRIBUTE.ToString();
                    break;
            }

            return action;
        }
    }
}
