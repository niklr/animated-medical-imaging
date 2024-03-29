﻿using AMI.Core.Constants;
using AMI.Core.Entities.Shared.Queries.GetPaginated;

namespace AMI.Core.Entities.AuditEvents.Queries.GetPaginated
{
    /// <summary>
    /// A validator for queries to get a list of paginated audit events.
    /// </summary>
    public class GetPaginatedQueryValidator : GetPaginatedQueryValidator<GetPaginatedQuery>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetPaginatedQueryValidator" /> class.
        /// </summary>
        /// <param name="constants">The application constants.</param>
        public GetPaginatedQueryValidator(IApplicationConstants constants)
            : base(constants)
        {
        }
    }
}
