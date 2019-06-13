using AMI.Core.Constants;
using AMI.Core.Entities.Shared.Queries.GetPaginated;

namespace AMI.Core.Entities.Objects.Queries.GetObjects
{
    /// <summary>
    /// A validator for queries to get a list of paginated objects.
    /// </summary>
    public class GetObjectsQueryValidator : GetPaginatedQueryValidator<GetObjectsQuery>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetObjectsQueryValidator" /> class.
        /// </summary>
        /// <param name="constants">The application constants.</param>
        public GetObjectsQueryValidator(IApplicationConstants constants)
            : base(constants)
        {
        }
    }
}
