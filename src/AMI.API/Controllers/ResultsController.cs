using System.IO;
using System.Net;
using System.Threading.Tasks;
using AMI.Core.Entities.Results.Queries;
using Microsoft.AspNetCore.Mvc;

namespace AMI.API.Controllers
{
    /// <summary>
    /// The endpoints related to results.
    /// </summary>
    /// <seealso cref="BaseController" />
    [ApiController]
    [Route("results")]
    public class ResultsController : BaseController
    {
        /// <summary>
        /// Gets the information of the object with the specified identifier.
        /// </summary>
        /// <param name="id">The identifier of the object.</param>
        /// <param name="filename">The name of the file.</param>
        /// <returns>The information of the object.</returns>
        [HttpGet("{id}/images/{filename}")]
        [ProducesResponseType(typeof(Stream), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetImage(string id, string filename)
        {
            var result = await Mediator.Send(new GetImageQuery { Id = id, Filename = filename }, CancellationToken);
            return File(result.FileContents, result.ContentType);
        }
    }
}
