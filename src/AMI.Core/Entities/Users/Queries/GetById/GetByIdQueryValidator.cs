using AMI.Core.Entities.Shared.Queries.GetEntity;

namespace AMI.Core.Entities.Users.Queries.GetById
{
    /// <summary>
    /// A validator for queries to get an entity by its identifier.
    /// </summary>
    public class GetByIdQueryValidator : GetEntityQueryValidator<GetByIdQuery>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetByIdQueryValidator"/> class.
        /// </summary>
        public GetByIdQueryValidator()
        {
        }
    }
}
