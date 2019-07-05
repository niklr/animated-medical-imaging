using System.Net;
using System.Threading.Tasks;
using AMI.Core.Entities.ApiSettings.Queries;
using Microsoft.AspNetCore.Mvc;
using Models = AMI.Core.Entities.Models;

namespace AMI.API.Controllers
{
    /// <summary>
    /// The endpoints related to API settings.
    /// </summary>
    [ApiController]
    [Route("api-settings")]
    public class ApiSettingsController : BaseController
    {
        /// <summary>
        /// Gets the API settings.
        /// </summary>
        /// <returns>The API settings.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(Models.ApiSettings), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get()
        {
            return Ok(await Mediator.Send(new GetQuery()));
        }
    }
}
