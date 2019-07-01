﻿using System.IO;
using System.Net;
using System.Threading.Tasks;
using AMI.Core.Entities.Results.Queries.GetById;
using AMI.Core.Entities.Results.Queries.GetImage;
using AMI.Core.Entities.Results.Queries.GetZip;
using Microsoft.AspNetCore.Mvc;
using Models = AMI.Core.Entities.Models;

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
        /// Gets the information of the result with the specified identifier.
        /// </summary>
        /// <param name="id">The identifier of the result.</param>
        /// <returns>The information of the result.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Models.ResultModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetById(string id)
        {
            return Ok(await Mediator.Send(new GetByIdQuery { Id = id }, CancellationToken));
        }

        /// <summary>
        /// Gets the image of the result with the specified identifier.
        /// </summary>
        /// <param name="id">The identifier of the object.</param>
        /// <param name="filename">The name of the file.</param>
        /// <returns>The information of the object.</returns>
        [HttpGet("{id}/images/{filename}")]
        [ProducesResponseType(typeof(Stream), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetImage(string id, string filename)
        {
            var result = await Mediator.Send(new GetImageQuery { Id = id, Filename = filename }, CancellationToken);
            return File(result.FileContents, result.ContentType, result.FileDownloadName);
        }

        /// <summary>
        /// Downloads the result with the specified identifier as a ZIP.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>The result as a ZIP.</returns>
        [HttpGet("{id}/download")]
        [ProducesResponseType(typeof(Stream), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DownloadById(string id)
        {
            var result = await Mediator.Send(new GetZipQuery { Id = id }, CancellationToken);
            return File(result.FileContents, result.ContentType, result.FileDownloadName);
        }
    }
}
