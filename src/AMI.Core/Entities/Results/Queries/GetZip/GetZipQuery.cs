using AMI.Core.Entities.Models;
using MediatR;

namespace AMI.Core.Entities.Results.Queries.GetZip
{
    /// <summary>
    /// A query to get the result as a zip.
    /// </summary>
    public class GetZipQuery : IRequest<FileByteResultModel>
    {
        /// <summary>
        /// Gets or sets the identifier of the result.
        /// </summary>
        public string Id { get; set; }
    }
}
