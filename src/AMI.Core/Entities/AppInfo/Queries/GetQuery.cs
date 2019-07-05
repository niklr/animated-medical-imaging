using MediatR;

namespace AMI.Core.Entities.AppInfo.Queries
{
    /// <summary>
    /// A query to get the application information.
    /// </summary>
    public class GetQuery : IRequest<Models.AppInfo>
    {
    }
}
