using AMI.Core.Extensions.FluentValidationExtensions;
using FluentValidation;

namespace AMI.Core.Entities.Results.Queries.GetZip
{
    /// <summary>
    /// A validator for queries to get a result as a zip.
    /// </summary>
    /// <seealso cref="AbstractValidator{GetImageQuery}" />
    public class GetZipQueryValidator : AbstractValidator<GetZipQuery>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetZipQueryValidator"/> class.
        /// </summary>
        public GetZipQueryValidator()
        {
            RuleFor(x => x.Id).NotEmpty().GuidValidation();
        }
    }
}
