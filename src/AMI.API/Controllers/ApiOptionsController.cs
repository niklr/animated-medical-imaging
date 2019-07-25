using System.Net;
using System.Threading.Tasks;
using AMI.Core.Entities.ApiOptions.Queries;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using Models = AMI.Core.Entities.Models;

namespace AMI.API.Controllers
{
    /// <summary>
    /// The endpoints related to API options.
    /// </summary>
    [ApiController]
    [Route("api-options")]
    public class ApiOptionsController : BaseController
    {
        /// <summary>
        /// Get API options
        /// </summary>
        /// <remarks>
        /// With this GET request you can obtain the options used to configure the API.
        /// It contains options related to authentication and authorization, rate limiting and others.
        /// </remarks>
        /// <returns>A model containing the API options.</returns>
        [HttpGet]
        [OpenApiTag("General")]
        [ProducesResponseType(typeof(Models.ApiOptions), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get()
        {
            return Ok(await Mediator.Send(new GetQuery()));
        }
    }
}
