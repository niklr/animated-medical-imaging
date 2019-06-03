using System.Net;
using System.Threading.Tasks;
using AMI.Core.Entities.Objects.Commands.Process;
using Microsoft.AspNetCore.Mvc;
using Models = AMI.Core.Entities.Models;

namespace AMI.API.Controllers
{
    /// <summary>
    /// The endpoints related to objects.
    /// </summary>
    /// <seealso cref="AMI.API.Controllers.BaseController" />
    [ApiController]
    [Route("objects")]
    public class ObjectsController : BaseController
    {
        /// <summary>
        /// Processes an existing object.
        /// </summary>
        /// <param name="command">The command to process an existing object.</param>
        /// <returns>The status of the processing.</returns>
        [HttpPut("{id}/process")]
        [ProducesResponseType(typeof(Models.ObjectResult), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ProcessAsync([FromBody] ProcessObjectCommand command)
        {
            return Ok(null);
        }
    }
}
