using AMI.Core.Entities.Models;
using AMI.Core.Factories;
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
        private readonly IAppInfoFactory appInfoFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeController"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="appInfoFactory">The factory.</param>
        public HomeController(IOptions<ClientOptions> configuration, IAppInfoFactory appInfoFactory)
        {
            this.configuration = configuration;
            this.appInfoFactory = appInfoFactory;
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

        /// <summary>
        /// Gets the information of the website.
        /// </summary>
        /// <returns>The website information.</returns>
        [HttpGet("app-info")]
        public AppInfoModel AppInfo()
        {
            return appInfoFactory.Create();
        }
    }
}