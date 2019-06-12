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
        public GetPaginatedQueryValidator()
        {
            RuleFor(x => x.Page).NotEmpty().GreaterThan(0);
            RuleFor(x => x.Limit).NotEmpty().GreaterThan(0);
        }
    }
}
