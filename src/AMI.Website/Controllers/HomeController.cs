using AMI.Website.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace AMI.Website.Controllers
{
    /// <summary>
    /// Home controller
    /// </summary>
    /// <seealso cref="Controller" />
    public class HomeController : Controller
    {
        private readonly IOptions<ClientSettings> configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeController"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public HomeController(IOptions<ClientSettings> configuration)
        {
            this.configuration = configuration;
        }

        /// <summary>
        /// Gets the settings of the website.
        /// </summary>
        /// <returns>The website settings.</returns>
        [HttpGet("settings")]
        public ClientSettings Settings()
        {
            return configuration.Value;
        }
    }
}