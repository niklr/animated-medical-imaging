using System;
using System.Net;
using System.Threading.Tasks;
using AMI.Core.Uploaders;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AMI.API.Controllers
{
    /// <summary>
    /// The endpoints related to upload.
    /// </summary>
    /// <seealso cref="BaseController" />
    [ApiController]
    [Route("upload")]
    public class UploadController : BaseController
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
        /// The resumable upload endpoint.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="chunkNumber">The chunk number.</param>
        /// <param name="chunkSize">The size of the chunk.</param>
        /// <param name="currentChunkSize">The size of the current chunk.</param>
        /// <param name="totalSize">The total size of the upload.</param>
        /// <param name="resumableType">The name of the file type.</param>
        /// <param name="uid">The unique identifier.</param>
        /// <param name="filename">The filename.</param>
        /// <param name="relativePath">The relative path.</param>
        /// <param name="totalChunks">The total chunks.</param>
        /// <returns>The result of the resumable upload.</returns>
        [HttpPost("resumable")]
        public async Task<IActionResult> ResumableUpload(
            IFormFile file,
            int chunkNumber,
            int chunkSize,
            int currentChunkSize,
            int totalSize,
            string resumableType,
            string uid,
            string filename,
            string relativePath,
            int totalChunks)
        {
            try
            {
                uploader.Upload(chunkNumber, uid, file.OpenReadStream());

                bool isFinalChunk = chunkNumber == totalChunks;
                if (isFinalChunk)
                {
                    await uploader.CommitAsync(filename, relativePath, uid, CancellationToken);
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
