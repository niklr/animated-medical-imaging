using FluentValidation;

namespace AMI.Core.Entities.Webhooks.Queries.GetByUser
{
    /// <summary>
    /// A validator for queries to get webhooks of a user.
    /// </summary>
    public class GetByUserQueryValidator : AbstractValidator<GetByUserQuery>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetByUserQueryValidator" /> class.
        /// </summary>
        public GetByUserQueryValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
        }
    }
}
