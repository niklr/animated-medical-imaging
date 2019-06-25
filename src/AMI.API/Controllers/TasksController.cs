using System.Net;
using System.Threading.Tasks;
using AMI.Core.Entities.Tasks.Commands.Create;
using AMI.Core.Entities.Tasks.Queries.GetById;
using Microsoft.AspNetCore.Mvc;
using Models = AMI.Core.Entities.Models;

namespace AMI.API.Controllers
{
    /// <summary>
    /// The endpoints related to tasks.
    /// </summary>
    /// <seealso cref="BaseController" />
    [ApiController]
    [Route("tasks")]
    public class TasksController : BaseController
    {
        /// <summary>
        /// Gets the information of the task with the specified identifier.
        /// </summary>
        /// <param name="id">The identifier of the task.</param>
        /// <returns>The information of the task.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Models.TaskModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetById(string id)
        {
            return Ok(await Mediator.Send(new GetByIdQuery { Id = id }, CancellationToken));
        }

        /// <summary>
        /// Creates a new task.
        /// </summary>
        /// <param name="command">The command to create a new task.</param>
        /// <returns>The created task.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(Models.TaskModel), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> Create([FromBody] CreateTaskCommand command)
        {
            var result = await Mediator.Send(command, CancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
    }
}
