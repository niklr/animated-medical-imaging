using MediatR;

namespace AMI.Core.Entities.ApiOptions.Queries
{
    /// <summary>
    /// A query to get the API options.
    /// </summary>
    public class GetQuery : IRequest<Models.ApiOptions>
    {
    }
}
