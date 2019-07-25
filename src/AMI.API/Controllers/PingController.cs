using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
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
        /// Get test value
        /// </summary>
        /// <remarks>
        /// With this GET request you can obtain a test value to verify the connection and authentication.
        /// If the ping is successful you will receive a pong in the form of a JSON response.
        /// </remarks>
        /// <returns>A model containing the test value.</returns>
        [HttpGet]
        [Authorize]
        [OpenApiTag("General")]
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
