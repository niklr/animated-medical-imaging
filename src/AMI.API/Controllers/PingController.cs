using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models = AMI.Core.Entities.Models;

namespace AMI.API.Controllers
{
    /// <summary>
    /// The endpoints related to testing.
    /// </summary>
    [ApiController]
    [Route("ping")]
    public class PingController : BaseController
    {
        /// <summary>
        /// Gets a test value to verify the connection and authentication.
        /// </summary>
        /// <returns>The test value if successful.</returns>
        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(Models.PongModel), (int)HttpStatusCode.OK)]
        public IActionResult Get()
        {
            var result = new Models.PongModel()
            {
                Pong = "Hello World"
            };
            return Ok(result);
        }
    }
}
