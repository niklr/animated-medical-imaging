using System;
using AMI.API.Extensions.HttpContextExtensions;
using AMI.Core.Configurations;
using Microsoft.AspNetCore.Mvc;

namespace AMI.API.Controllers
{
    /// <summary>
    /// Home controller
    /// </summary>
    /// <seealso cref="Controller" />
    [Route("")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class HomeController : Controller
    {
        private readonly IApiConfiguration configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeController"/> class.
        /// </summary>
        /// <param name="configuration">The API configuration.</param>
        /// <exception cref="ArgumentNullException">configuration</exception>
        public HomeController(IApiConfiguration configuration)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        /// <summary>
        /// Redirects to the default page.
        /// </summary>
        /// <returns>The default page.</returns>
        public IActionResult Index()
        {
            return Redirect("/redoc");
        }

        /// <summary>
        /// Gets the test information.
        /// </summary>
        /// <returns>The test information.</returns>
        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok(HttpContext?.GetRemoteIpAddress(configuration));
        }
    }
}