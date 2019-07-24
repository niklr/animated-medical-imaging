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
        /// Gets the application options.
        /// </summary>
        /// <returns>The application options.</returns>
        [HttpGet]
        [OpenApiTag("General")]
        [ProducesResponseType(typeof(Models.AppOptions), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get()
        {
            return Ok(await Mediator.Send(new GetQuery()));
        }
    }
}
