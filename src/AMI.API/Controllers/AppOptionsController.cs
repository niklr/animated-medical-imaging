using System.Net;
using System.Threading.Tasks;
using AMI.Core.Entities.AppOptions.Queries;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using Models = AMI.Core.Entities.Models;

namespace AMI.API.Controllers
{
    /// <summary>
    /// The endpoints related to application options.
    /// </summary>
    [ApiController]
    [Route("app-options")]
    public class AppOptionsController : BaseController
    {
        /// <summary>
        /// Get application options
        /// </summary>
        /// <remarks>
        /// With this GET request you can obtain the options used to configure the application.
        /// It contains options such as the timeout in milliseconds before a request is canceled,
        /// the maximum amount of entries an archive is allowed to have, and others.
        /// </remarks>
        /// <returns>A model containing the application options.</returns>
        [HttpGet]
        [OpenApiTag("General")]
        [ProducesResponseType(typeof(Models.AppOptions), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get()
        {
            return Ok(await Mediator.Send(new GetQuery()));
        }
    }
}
