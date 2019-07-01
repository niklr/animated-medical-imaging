using AMI.Core.Extensions.FluentValidationExtensions;
using FluentValidation;

namespace AMI.Core.Entities.Results.Queries.GetById
{
    /// <summary>
    /// A validator for queries to get a result by its identifier.
    /// </summary>
    /// <seealso cref="AbstractValidator{GetByIdQuery}" />
    public class GetByIdQueryValidator : AbstractValidator<GetByIdQuery>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetByIdQueryValidator"/> class.
        /// </summary>
        public GetByIdQueryValidator()
        {
            RuleFor(x => x.Id).NotEmpty().GuidValidation();
        }
    }
}
