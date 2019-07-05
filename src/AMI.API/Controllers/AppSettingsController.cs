using System.Net;
using System.Threading.Tasks;
using AMI.Core.Entities.ApplicationSettings.Queries;
using Microsoft.AspNetCore.Mvc;
using Models = AMI.Core.Entities.Models;

namespace AMI.API.Controllers
{
    /// <summary>
    /// The endpoints related to application settings.
    /// </summary>
    [ApiController]
    [Route("app-settings")]
    public class AppSettingsController : BaseController
    {
        /// <summary>
        /// Gets the application settings.
        /// </summary>
        /// <returns>The application settings.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(Models.AppSettings), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get()
        {
            return Ok(await Mediator.Send(new GetQuery()));
        }
    }
}
