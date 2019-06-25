using AMI.Core.Entities.Models;
using AMI.Core.Entities.Shared.Commands.ProcessObjectAsync;
using AMI.Core.Extensions.FluentValidationExtensions;
using FluentValidation;

namespace AMI.Core.Entities.Results.Commands.ProcessObject
{
    /// <summary>
    /// A validator for process command requests.
    /// </summary>
    /// <seealso cref="AbstractValidator{ProcessObjectCommand}" />
    public class ProcessCommandValidator : AbstractValidator<ProcessObjectCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessCommandValidator"/> class.
        /// </summary>
        public ProcessCommandValidator()
        {
            Include(new BaseProcessCommandValidator<ProcessResultModel>());
            RuleFor(x => x.Id).NotEmpty().GuidValidation();
        }
    }
}
