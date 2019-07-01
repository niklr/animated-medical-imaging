using AMI.Core.Extensions.FluentValidationExtensions;
using FluentValidation;

namespace AMI.Core.Entities.Results.Queries.GetImage
{
    /// <summary>
    /// A validator for queries to get an image file result.
    /// </summary>
    /// <seealso cref="AbstractValidator{GetImageQuery}" />
    public class GetImageQueryValidator : AbstractValidator<GetImageQuery>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetImageQueryValidator"/> class.
        /// </summary>
        public GetImageQueryValidator()
        {
            RuleFor(x => x.Id).NotEmpty().GuidValidation();
            RuleFor(x => x.Filename).NotEmpty();
        }
    }
}
