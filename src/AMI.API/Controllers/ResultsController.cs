using System.Net;
using System.Threading.Tasks;
using AMI.Core.Entities.Results.Queries.GetById;
using AMI.Core.Entities.Results.Queries.GetImage;
using AMI.Core.Entities.Results.Queries.GetZip;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models = AMI.Core.Entities.Models;

namespace AMI.API.Controllers
{
    /// <summary>
    /// The endpoints related to results.
    /// </summary>
    [ApiController]
    [Route("results")]
    public class ResultsController : BaseController
    {
        /// <summary>
        /// Get result by id
        /// </summary>
        /// <param name="id">The identifier of the result.</param>
        /// <remarks>
        /// With this GET request you can obtain information about the result with the specified identifier.
        /// A result is an abstraction for the outcome of a finished task.
        /// </remarks>
        /// <returns>A model containing the specified result.</returns>
        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(typeof(Models.ResultModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetById(string id)
        {
            return Ok(await Mediator.Send(new GetByIdQuery { Id = id }, CancellationToken));
        }

        /// <summary>
        /// Get image of result
        /// </summary>
        /// <param name="id">The identifier of the object.</param>
        /// <param name="filename">The name of the file.</param>
        /// <remarks>
        /// With this GET request you can obtain a specific image of the result with the specified identifier.
        /// </remarks>
        /// <returns>A stream containing the image.</returns>
        [HttpGet("{id}/images/{filename}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetImage(string id, string filename)
        {
            var result = await Mediator.Send(new GetImageQuery { Id = id, Filename = filename }, CancellationToken);
            return File(result.FileContents, result.ContentType, result.FileDownloadName);
        }

        /// <summary>
        /// Download result
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <remarks>
        /// With this GET request you can download the result with the specified identifier as an archive.
        /// </remarks>
        /// <returns>A stream containing the result as an archive.</returns>
        [HttpGet("{id}/download")]
        [Authorize]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> DownloadById(string id)
        {
            var result = await Mediator.Send(new GetZipQuery { Id = id }, CancellationToken);
            return File(result.FileContents, result.ContentType, result.FileDownloadName);
        }
    }
}
