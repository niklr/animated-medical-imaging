using System;
using System.Net;
using System.Threading.Tasks;
using AMI.Core.Entities.Objects.Commands.Delete;
using AMI.Core.Entities.Objects.Queries.GetById;
using AMI.Core.Entities.Objects.Queries.GetObjects;
using AMI.Core.IO.Uploaders;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RNS.Framework.Threading;
using Models = AMI.Core.Entities.Models;

namespace AMI.API.Controllers
{
    /// <summary>
    /// The endpoints related to objects.
    /// </summary>
    /// <seealso cref="BaseController" />
    [ApiController]
    [Route("objects")]
    public class ObjectsController : BaseController
    {
        private readonly IChunkedObjectUploader uploader;

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectsController"/> class.
        /// </summary>
        /// <param name="uploader">The resumable uploader.</param>
        public ObjectsController(IChunkedObjectUploader uploader)
        {
            this.uploader = uploader ?? throw new ArgumentNullException(nameof(uploader));
        }

        /// <summary>
        /// Gets a paginated list of objects.
        /// </summary>
        /// <param name="page">The current page.</param>
        /// <param name="limit">The limit to constrain the number of items.</param>
        /// <returns>The list of paginated objects.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(Models.PaginationResultModel<Models.ObjectModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetPaginatedAsync(int page, int limit)
        {
            return Ok(await Mediator.Send(new GetObjectsQuery { Page = page, Limit = limit }, CancellationToken));
        }

        /// <summary>
        /// Gets the information of the object with the specified identifier.
        /// </summary>
        /// <param name="id">The identifier of the object.</param>
        /// <returns>The information of the object.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Models.ObjectModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetById(string id)
        {
            return Ok(await Mediator.Send(new GetByIdQuery { Id = id }, CancellationToken));
        }

        /// <summary>
        /// Deletes the object with the specified identifier.
        /// </summary>
        /// <param name="id">The identifier of the object.</param>
        /// <returns>No content.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Models.ObjectModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteById(string id)
        {
            await Mediator.Send(new DeleteObjectCommand { Id = id }, CancellationToken);
            return NoContent();
        }

        /// <summary>
        /// Uploads an object.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="chunkNumber">The chunk number.</param>
        /// <param name="totalSize">The total size of the upload.</param>
        /// <param name="uid">The unique identifier.</param>
        /// <param name="filename">The filename.</param>
        /// <param name="relativePath">The relative path.</param>
        /// <param name="totalChunks">The total chunks.</param>
        /// <returns>The result of the resumable upload.</returns>
        [HttpPost("upload")]
        [ProducesResponseType(typeof(Models.ObjectModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UploadAsync(
            IFormFile file,
            int chunkNumber,
            int totalSize,
            string uid,
            string filename,
            string relativePath,
            int totalChunks)
        {
            try
            {
                if (!Guid.TryParse(uid, out Guid parsedUid))
                {
                    throw new ArgumentException("The provided unique identifier is not valid.");
                }

                ThreadThrottler throttler = new ThreadThrottler();

                var chunkResult = await uploader.UploadAsync(totalChunks, chunkNumber, uid, file.OpenReadStream(), CancellationToken);

                // Ensure current request lasts at least 500 milliseconds
                throttler.Throttle(500);

                if (chunkResult != null && chunkResult.IsCompleted)
                {
                    var result = await uploader.CommitAsync(filename, relativePath, uid, CancellationToken);
                    return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
                }
                else
                {
                    return Content(string.Empty);
                }
            }
            catch (ArgumentException e)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Content(e.Message);
            }
            catch (Exception e)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return Content(e.Message);
            }
        }
    }
}
