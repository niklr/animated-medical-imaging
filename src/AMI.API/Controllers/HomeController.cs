using Microsoft.AspNetCore.Mvc;

namespace AMI.API.Controllers
{
    /// <summary>
    /// Home controller
    /// </summary>
    /// <seealso cref="Controller" />
    [Route("")]
    public class HomeController : Controller
    {
        /// <summary>
        /// Redirects to the default page.
        /// </summary>
        /// <returns>The default page.</returns>
        public IActionResult Index()
        {
            return Redirect("/redoc");
        }
    }
}