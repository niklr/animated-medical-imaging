using FluentValidation;
using ResultCommands = AMI.Core.Entities.Results.Commands;

namespace AMI.Core.Entities.Tasks.Commands.Create
{
    /// <summary>
    /// A validator for create command requests.
    /// </summary>
    /// <seealso cref="AbstractValidator{CreateTaskCommand}" />
    public class CreateCommandValidator : AbstractValidator<CreateTaskCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateCommandValidator"/> class.
        /// </summary>
        public CreateCommandValidator()
        {
            RuleFor(x => x.Command).NotNull();
            When(x => x.Command?.GetType() == typeof(ResultCommands.ProcessObject.ProcessObjectCommand), () =>
            {
                RuleFor(x => (ResultCommands.ProcessObject.ProcessObjectCommand)x.Command).SetValidator(new ResultCommands.ProcessObject.ProcessCommandValidator());
            });
            When(x => x.Command?.GetType() == typeof(ResultCommands.ProcessPath.ProcessPathCommand), () =>
            {
                RuleFor(x => (ResultCommands.ProcessPath.ProcessPathCommand)x.Command).SetValidator(new ResultCommands.ProcessPath.ProcessCommandValidator());
            });
        }
    }
}
