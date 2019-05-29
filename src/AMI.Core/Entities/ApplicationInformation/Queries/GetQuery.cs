using AMI.Core.Entities.Models;
using MediatR;

namespace AMI.Core.Entities.ApplicationInformation.Queries
{
    /// <summary>
    /// A query to get the application information.
    /// </summary>
    /// <seealso cref="IRequest{AppInfo}" />
    public class GetQuery : IRequest<AppInfo>
    {
    }
}
