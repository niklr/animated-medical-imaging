using System.Net;
using System.Threading.Tasks;
using AMI.Core.Entities.AppInfo.Queries;
using Microsoft.AspNetCore.Mvc;
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
        /// Gets the application information.
        /// </summary>
        /// <returns>The application information.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(Models.AppInfo), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get()
        {
            return Ok(await Mediator.Send(new GetQuery()));
        }
    }
}
