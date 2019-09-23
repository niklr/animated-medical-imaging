using System.Collections.Generic;
using System.Linq;
using AMI.Core.Extensions.FluentValidationExtensions;
using AMI.Domain.Enums;
using FluentValidation;
using RNS.Framework.Tools;

namespace AMI.Core.Entities.Webhooks.Commands
{
    /// <summary>
    /// A base validator for webhook command requests.
    /// </summary>
    /// <typeparam name="T">The type of the response.</typeparam>
    public class BaseCommandValidator<T> : AbstractValidator<BaseWebhookCommand<T>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseCommandValidator{T}"/> class.
        /// </summary>
        public BaseCommandValidator()
        {
            var allowedEvents = new HashSet<string>(EnumUtil.GetValues<EventType>().Select(x => EnumUtil.GetString(x)))
            {
                "*"
            }.ToArray();

            RuleFor(x => x.Url).NotEmpty().MaximumLength(2048).UrlValidation();
            RuleFor(x => x.ApiVersion).NotEmpty().MaximumLength(128);
            RuleFor(x => x.Secret).NotEmpty().MaximumLength(4096);
            RuleFor(x => x.EnabledEvents).NotEmpty();
            RuleForEach(x => x.EnabledEvents).NotEmpty().In(allowedEvents);
        }
    }
}
