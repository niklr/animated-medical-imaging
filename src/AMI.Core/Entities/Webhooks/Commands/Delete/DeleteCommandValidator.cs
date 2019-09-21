using FluentValidation;

namespace AMI.Core.Entities.Webhooks.Commands.Delete
{
    /// <summary>
    /// A validator for delete command requests.
    /// </summary>
    public class DeleteCommandValidator : AbstractValidator<DeleteWebhookCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteCommandValidator"/> class.
        /// </summary>
        public DeleteCommandValidator()
        {
        }
    }
}
