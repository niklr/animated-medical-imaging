using System.Net;
using System.Threading.Tasks;
using AMI.API.Attributes;
using AMI.Core.Entities.AuditEvents.Queries.GetPaginated;
using AMI.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using Models = AMI.Core.Entities.Models;

namespace AMI.API.Controllers
{
    /// <summary>
    /// Audit event management
    /// </summary>
    [ApiController]
    [RequiresRole(RoleType.Administrator)]
    [Route("audit-events")]
    public class AuditEventsController : BaseController
    {
        /// <summary>
        /// Get paginated list of audit events
        /// </summary>
        /// <param name="page">The current page.</param>
        /// <param name="limit">The limit to constrain the number of items.</param>
        /// <remarks>
        /// With this GET request you can obtain a paginated list of audit events.
        /// The audit events are sorted in descending order by timestamp.
        /// </remarks>
        /// <returns>A model containing a list of paginated audit events.</returns>
        [HttpGet]
        [OpenApiTag("Admin")]
        [ProducesResponseType(typeof(Models.PaginationResultModel<Models.AuditEventModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetPaginatedAsync(int page, int limit)
        {
            return Ok(await Mediator.Send(new GetPaginatedQuery { Page = page, Limit = limit }, CancellationToken));
        }
    }
}
