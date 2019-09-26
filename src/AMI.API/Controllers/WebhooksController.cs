using System.Net;
using System.Threading.Tasks;
using AMI.Core.Entities.Webhooks.Commands.Create;
using AMI.Core.Entities.Webhooks.Commands.Delete;
using AMI.Core.Entities.Webhooks.Commands.Update;
using AMI.Core.Entities.Webhooks.Queries.GetById;
using AMI.Core.Entities.Webhooks.Queries.GetPaginated;
using AMI.Domain.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models = AMI.Core.Entities.Models;

namespace AMI.API.Controllers
{
    /// <summary>
    /// The endpoints related to webhooks.
    /// </summary>
    [ApiController]
    [Route("webhooks")]
    public class WebhooksController : BaseController
    {
        /// <summary>
        /// Get webhook by id
        /// </summary>
        /// <param name="id">The identifier of the webhook.</param>
        /// <remarks>
        /// With this GET request you can obtain information about the webhook with the specified identifier.
        /// </remarks>
        /// <returns>A model containing the specified webhook.</returns>
        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(typeof(Models.WebhookModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetById(string id)
        {
            return Ok(await Mediator.Send(new GetByIdQuery { Id = id }, CancellationToken));
        }

        /// <summary>
        /// Get paginated list of webhooks
        /// </summary>
        /// <param name="page">The current page.</param>
        /// <param name="limit">The limit to constrain the number of items.</param>
        /// <remarks>
        /// With this GET request you can obtain a paginated list of webhooks.
        /// The webhooks are sorted in descending order by creation date.
        /// </remarks>
        /// <returns>A model containing a list of paginated webhooks.</returns>
        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(Models.PaginationResultModel<Models.WebhookModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetPaginatedAsync(int page, int limit)
        {
            return Ok(await Mediator.Send(new GetPaginatedQuery { Page = page, Limit = limit }, CancellationToken));
        }

        /// <summary>
        /// Create webhook
        /// </summary>
        /// <param name="command">The command to create a new webhook.</param>
        /// <remarks>
        /// With this POST request you can create a webhook.
        /// </remarks>
        /// <returns>A model containing the created webhook.</returns>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(Models.WebhookModel), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> Create([FromBody] CreateWebhookCommand command)
        {
            var result = await Mediator.Send(command, CancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        /// <summary>
        /// Update webhook
        /// </summary>
        /// <param name="id">The identifier of the webhook.</param>
        /// <param name="command">The command to update an existing webhook.</param>
        /// <remarks>
        /// With this PUT request you can update a webhook.
        /// </remarks>
        /// <returns>A model containing the updated webhook.</returns>
        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType(typeof(Models.WebhookModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateWebhookCommand command)
        {
            if (!id.Equals(command.Id))
            {
                throw new AmiException("The specified identifiers do not match.");
            }

            return Ok(await Mediator.Send(command, CancellationToken));
        }

        /// <summary>
        /// Delete webhook by id
        /// </summary>
        /// <param name="id">The identifier of the webhook.</param>
        /// <remarks>
        /// With this DELETE request you can delete the webhook with the specified identifier.
        /// </remarks>
        /// <returns>No content.</returns>
        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(typeof(Models.WebhookModel), (int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> DeleteById(string id)
        {
            await Mediator.Send(new DeleteWebhookCommand { Id = id }, CancellationToken);
            return NoContent();
        }
    }
}
