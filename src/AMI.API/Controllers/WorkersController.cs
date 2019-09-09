using System.Net;
using System.Threading.Tasks;
using AMI.API.Attributes;
using AMI.Core.Entities.Workers.Queries.GetPaginated;
using AMI.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using Models = AMI.Core.Entities.Models;

namespace AMI.API.Controllers
{
    /// <summary>
    /// Application log management
    /// </summary>
    [ApiController]
    // [RequiresRole(RoleType.Administrator)]
    [Route("workers")]
    public class WorkersController : BaseController
    {
        /// <summary>
        /// Get paginated list of workers
        /// </summary>
        /// <param name="page">The current page.</param>
        /// <param name="limit">The limit to constrain the number of items.</param>
        /// <remarks>
        /// With this GET request you can obtain a paginated list of workers.
        /// The workers are sorted in ascending order by name.
        /// </remarks>
        /// <returns>A model containing a list of paginated workers.</returns>
        [HttpGet]
        [OpenApiTag("Admin")]
        [ProducesResponseType(typeof(Models.PaginationResultModel<Models.BaseWorkerModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetPaginatedAsync(int page, int limit)
        {
            return Ok(await Mediator.Send(new GetPaginatedQuery { Page = page, Limit = limit }, CancellationToken));
        }
    }
}
