using System;
using AMI.Core.Constants;
using AMI.Core.Extensions.FluentValidationExtensions;
using FluentValidation;

namespace AMI.Core.Entities.Shared.Queries.GetPaginated
{
    /// <summary>
    /// A validator for queries to get a paginated list of entities.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    public class GetPaginatedQueryValidator<T> : AbstractValidator<T>
        where T : IGetPaginatedQuery
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetPaginatedQueryValidator{T}"/> class.
        /// </summary>
        /// <param name="constants">The application constants.</param>
        public GetPaginatedQueryValidator(IApplicationConstants constants)
        {
            if (constants == null)
            {
                throw new ArgumentNullException(nameof(constants));
            }

            RuleFor(x => x.Page).NotEmpty().GreaterThan(0);
            RuleFor(x => x.Limit).NotEmpty().GreaterThan(0).In(constants.AllowedPaginationLimitValues);
        }
    }
}
