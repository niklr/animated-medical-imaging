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
        private readonly IOptions<ClientOptions> configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeController"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public HomeController(IOptions<ClientOptions> configuration)
        {
            this.configuration = configuration;
        }

        /// <summary>
        /// Gets the options of the website.
        /// </summary>
        /// <returns>The website options.</returns>
        [HttpGet("options")]
        public ClientOptions Options()
        {
            return configuration.Value;
        }
    }
}