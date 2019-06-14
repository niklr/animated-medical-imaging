using System;
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
            RuleFor(x => x.Id).NotEmpty().Must(x =>
            {
                return Guid.TryParse(x, out Guid result);
            });
        }
    }
}
