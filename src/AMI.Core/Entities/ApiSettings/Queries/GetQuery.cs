using MediatR;

namespace AMI.Core.Entities.ApiSettings.Queries
{
    /// <summary>
    /// A query to get the API settings.
    /// </summary>
    public class GetQuery : IRequest<Models.ApiSettings>
    {
    }
}
