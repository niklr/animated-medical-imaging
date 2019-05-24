using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Uploaders;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AMI.API.Controllers
{
    /// <summary>
    /// The endpoints related to upload.
    /// </summary>
    /// <seealso cref="ControllerBase" />
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly IResumableUploader uploader;

        /// <summary>
        /// Initializes a new instance of the <see cref="UploadController"/> class.
        /// </summary>
        /// <param name="uploader">The resumable uploader.</param>
        public UploadController(IResumableUploader uploader)
        {
            this.uploader = uploader;
        }

        /// <summary>
        /// Gets a list of values.
        /// </summary>
        /// <returns>A list of values.</returns>
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        /// <summary>
        /// The resumable upload endpoint.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="resumableChunkNumber">The resumable chunk number.</param>
        /// <param name="resumableChunkSize">The size of the resumable chunk.</param>
        /// <param name="resumableCurrentChunkSize">The size of the resumable current chunk.</param>
        /// <param name="resumableTotalSize">The total size of the resumable upload.</param>
        /// <param name="resumableType">The name of the file type.</param>
        /// <param name="resumableIdentifier">The resumable identifier.</param>
        /// <param name="resumableFilename">The resumable filename.</param>
        /// <param name="resumableRelativePath">The resumable relative path.</param>
        /// <param name="resumableTotalChunks">The resumable total chunks.</param>
        /// <param name="ct">The cancellation token.</param>
        /// <returns>The result of the resumable upload.</returns>
        [HttpPost]
        public async Task<IActionResult> ResumableUpload(
            IFormFile file,
            int resumableChunkNumber,
            int resumableChunkSize,
            int resumableCurrentChunkSize,
            int resumableTotalSize,
            string resumableType,
            string resumableIdentifier,
            string resumableFilename,
            string resumableRelativePath,
            int resumableTotalChunks,
            CancellationToken ct)
        {
            try
            {
                uploader.Upload(resumableChunkNumber, resumableIdentifier, file.OpenReadStream());

                bool isFinalChunk = resumableChunkNumber == resumableTotalChunks;
                if (isFinalChunk)
                {
                    await uploader.CommitAsync(resumableFilename, resumableRelativePath, resumableIdentifier, ct);
                }

                return Content(string.Empty);
            }
            catch (Exception e)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return Content(e.Message);
            }
        }
    }
}
