using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AMI.Website.Pages
{
    /// <summary>
    /// A page model to display errors.
    /// </summary>
    /// <seealso cref="PageModel" />
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
#pragma warning disable SA1649 // File name must match first type name
    public class ErrorModel : PageModel
#pragma warning restore SA1649 // File name must match first type name
    {
        /// <summary>
        /// Gets or sets the request identifier.
        /// </summary>
        public string RequestId { get; set; }

        /// <summary>
        /// Gets a value indicating whether [show request identifier].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [show request identifier]; otherwise, <c>false</c>.
        /// </value>
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        /// <summary>
        /// Sets the request identifier.
        /// </summary>
        public void OnGet()
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
        }
    }
}