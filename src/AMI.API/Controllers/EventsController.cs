using System.Net;
using System.Threading.Tasks;
using AMI.Core.Entities.Events.Queries.GetPaginated;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models = AMI.Core.Entities.Models;

namespace AMI.API.Controllers
{
    /// <summary>
    /// Events management
    /// </summary>
    [ApiController]
    [Route("events")]
    public class EventsController : BaseController
    {
        /// <summary>
        /// Get paginated list of events
        /// </summary>
        /// <param name="page">The current page.</param>
        /// <param name="limit">The limit to constrain the number of items.</param>
        /// <remarks>
        /// With this GET request you can obtain a paginated list of events.
        /// The events are sorted in descending order by creation date.
        /// </remarks>
        /// <returns>A model containing a list of paginated events.</returns>
        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(Models.PaginationResultModel<Models.EventModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetPaginatedAsync(int page, int limit)
        {
            return Ok(await Mediator.Send(new GetPaginatedQuery { Page = page, Limit = limit }, CancellationToken));
        }
    }
}
