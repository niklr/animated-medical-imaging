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
        /// Gets the API options.
        /// </summary>
        /// <returns>The API options.</returns>
        [HttpGet]
        [OpenApiTag("General")]
        [ProducesResponseType(typeof(Models.ApiOptions), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get()
        {
            return Ok(await Mediator.Send(new GetQuery()));
        }
    }
}
