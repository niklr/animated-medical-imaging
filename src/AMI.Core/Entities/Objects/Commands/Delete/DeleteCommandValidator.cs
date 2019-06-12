using System;
using FluentValidation;

namespace AMI.Core.Entities.Objects.Commands.Delete
{
    /// <summary>
    /// A validator for delect command requests.
    /// </summary>
    /// <seealso cref="AbstractValidator{DeleteObjectCommand}" />
    public class DeleteCommandValidator : AbstractValidator<DeleteObjectCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteCommandValidator"/> class.
        /// </summary>
        public DeleteCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().Must(x =>
            {
                return Guid.TryParse(x, out Guid result);
            });
        }
    }
}
