using AMI.Core.Entities.Models;
using MediatR;

namespace AMI.Core.Entities.ApplicationSettings.Queries
{
    /// <summary>
    /// A query to get the application settings.
    /// </summary>
    /// <seealso cref="IRequest{AppSettings}" />
    public class GetQuery : IRequest<AppSettings>
    {
    }
}
