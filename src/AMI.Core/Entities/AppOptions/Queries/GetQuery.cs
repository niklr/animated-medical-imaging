using MediatR;

namespace AMI.Core.Entities.AppOptions.Queries
{
    /// <summary>
    /// A query to get the application options.
    /// </summary>
    public class GetQuery : IRequest<Models.AppOptions>
    {
    }
}
