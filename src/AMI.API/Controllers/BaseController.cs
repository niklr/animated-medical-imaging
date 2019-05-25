using System.Threading;
using Microsoft.AspNetCore.Mvc;

namespace AMI.API.Controllers
{
    /// <summary>
    /// Base controller
    /// </summary>
    [ApiController]
    public abstract class BaseController : Controller
    {
        /// <summary>
        /// Gets the cancellation token.
        /// </summary>
        protected CancellationToken CancellationToken => HttpContext?.RequestAborted ?? CancellationToken.None;
    }
}
