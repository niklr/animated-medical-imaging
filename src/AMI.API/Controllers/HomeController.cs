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
    public class HomeController : BaseController
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
            return Redirect("/swagger");
        }

        /// <summary>
        /// Gets the remote IP address.
        /// </summary>
        /// <returns>The remote IP address.</returns>
        [HttpGet("myip")]
        public IActionResult MyIp()
        {
            return Ok(HttpContext?.GetRemoteIpAddress(configuration));
        }

        /// <summary>
        /// Gets the remote IP address.
        /// </summary>
        /// <returns>The remote IP address.</returns>
        [HttpGet("baseurl")]
        public IActionResult BaseUrl()
        {
            return Ok(AppBaseUrl);
        }
    }
}