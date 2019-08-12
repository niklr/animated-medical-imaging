using FluentValidation;

namespace AMI.Core.Entities.Shared.Commands
{
    /// <summary>
    /// A base validator for process command requests.
    /// </summary>
    /// <typeparam name="T">The type of the response.</typeparam>
    public class BaseProcessCommandValidator<T> : AbstractValidator<BaseProcessCommand<T>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseProcessCommandValidator{T}"/> class.
        /// </summary>
        public BaseProcessCommandValidator()
        {
            RuleFor(x => x.AmountPerAxis).NotNull().GreaterThan(0);
            RuleFor(x => x.Delay).NotNull().GreaterThanOrEqualTo(1).LessThanOrEqualTo(100);
            RuleFor(x => x.OutputSize).NotNull().GreaterThanOrEqualTo(0);
        }
    }
}
