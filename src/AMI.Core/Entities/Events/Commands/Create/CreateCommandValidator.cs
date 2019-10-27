using FluentValidation;

namespace AMI.Core.Entities.Events.Commands.Create
{
    /// <summary>
    /// A validator for create command requests.
    /// </summary>
    public class CreateCommandValidator : AbstractValidator<CreateEventCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateCommandValidator"/> class.
        /// </summary>
        public CreateCommandValidator()
        {
            RuleFor(x => x.Event).NotNull();
        }
    }
}
