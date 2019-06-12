﻿using System;
using System.Net;
using System.Threading.Tasks;
using AMI.Core.Entities.Results.Commands.ProcessObjects;
using AMI.Core.IO.Uploaders;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models = AMI.Core.Entities.Models;

namespace AMI.API.Controllers
{
    /// <summary>
    /// The endpoints related to objects.
    /// </summary>
    /// <seealso cref="AMI.API.Controllers.BaseController" />
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
            this.uploader = uploader;
        }

        /// <summary>
        /// Gets the information of the object with the specified identifier.
        /// </summary>
        /// <param name="id">The identifier of the object.</param>
        /// <returns>The information of the object.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Models.ObjectModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetById(string id)
        {
            return Ok(null);
        }

        /// <summary>
        /// Processes an existing object.
        /// </summary>
        /// <param name="id">The identifier of the object.</param>
        /// <param name="command">The command to process an existing object.</param>
        /// <returns>The created task.</returns>
        [HttpPut("{id}/process")]
        [ProducesResponseType(typeof(Models.TaskModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ProcessAsync(string id, [FromBody] ProcessObjectCommand command)
        {
            return Ok(null);
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

                var chunkResult = await uploader.UploadAsync(totalChunks, chunkNumber, uid, file.OpenReadStream(), CancellationToken);

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
