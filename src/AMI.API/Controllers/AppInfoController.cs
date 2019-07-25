using System.Net;
using System.Threading.Tasks;
using AMI.Core.Entities.AppInfo.Queries;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using Models = AMI.Core.Entities.Models;

namespace AMI.API.Controllers
{
    /// <summary>
    /// Application information management
    /// </summary>
    [ApiController]
    [Route("app-info")]
    public class AppInfoController : BaseController
    {
        /// <summary>
        /// Get application information
        /// </summary>
        /// <remarks>
        /// With this GET request you can obtain information about the application (e.g. application name and current version).
        /// </remarks>
        /// <returns>A model containing the application information.</returns>
        [HttpGet]
        [OpenApiTag("General")]
        [ProducesResponseType(typeof(Models.AppInfo), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get()
        {
            return Ok(await Mediator.Send(new GetQuery()));
        }
    }
}
