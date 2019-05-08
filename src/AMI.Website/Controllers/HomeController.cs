using System.Diagnostics;
using AMI.Website.Models;
using Microsoft.AspNetCore.Mvc;

namespace AMI.Website.Controllers
{
    /// <summary>
    /// The home controller.
    /// </summary>
    /// <seealso cref="Controller" />
    public class HomeController : Controller
    {
        /// <summary>
        /// Gets the index view.
        /// </summary>
        /// <returns>The index view.</returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Gets the privacy view.
        /// </summary>
        /// <returns>The privacy view.</returns>
        public IActionResult Privacy()
        {
            return View();
        }

        /// <summary>
        /// Gets the error view.
        /// </summary>
        /// <returns>The error view.</returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
