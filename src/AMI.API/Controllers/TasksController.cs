using System.Net;
using System.Threading.Tasks;
using AMI.Core.Entities.Tasks.Commands.Create;
using AMI.Core.Entities.Tasks.Queries.GetById;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models = AMI.Core.Entities.Models;

namespace AMI.API.Controllers
{
    /// <summary>
    /// The endpoints related to tasks.
    /// </summary>
    [ApiController]
    [Route("tasks")]
    public class TasksController : BaseController
    {
        /// <summary>
        /// Get task by id
        /// </summary>
        /// <param name="id">The identifier of the task.</param>
        /// <remarks>
        /// With this GET request you can obtain information about the task with the specified identifier.
        /// A task is an abstraction for a job being processed in the background.
        /// </remarks>
        /// <returns>A model containing the specified task.</returns>
        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(typeof(Models.TaskModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetById(string id)
        {
            return Ok(await Mediator.Send(new GetByIdQuery { Id = id }, CancellationToken));
        }

        /// <summary>
        /// Create task
        /// </summary>
        /// <param name="command">The command to create a new task.</param>
        /// <remarks>
        /// With this POST request you can create a task being processed in the background.
        /// </remarks>
        /// <returns>A model containing the created task.</returns>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(Models.TaskModel), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> Create([FromBody] CreateTaskCommand command)
        {
            var result = await Mediator.Send(command, CancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
    }
}
