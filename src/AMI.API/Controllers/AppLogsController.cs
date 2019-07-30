using System.Net;
using System.Threading.Tasks;
using AMI.API.Attributes;
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
    [RequiresRole(RoleType.Administrator)]
    [Route("app-logs")]
    public class AppLogsController : BaseController
    {
        /// <summary>
        /// Get paginated list of application logs
        /// </summary>
        /// <param name="page">The current page.</param>
        /// <param name="limit">The limit to constrain the number of items.</param>
        /// <remarks>
        /// With this GET request you can obtain a paginated list of application logs.
        /// The application logs are sorted in descending order by creation date.
        /// </remarks>
        /// <returns>A model containing a list of paginated application logs.</returns>
        [HttpGet]
        [OpenApiTag("Admin")]
        [ProducesResponseType(typeof(Models.PaginationResultModel<Models.ObjectModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetPaginatedAsync(int page, int limit)
        {
            return Ok(null);
        }
    }
}
