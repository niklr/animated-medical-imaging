using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AMI.API.ViewModels;
using AMI.Core.Constants;
using AMI.Core.Entities.Models;
using AMI.Domain.Entities;
using AMI.Domain.Exceptions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AMI.API.Controllers
{
    /// <summary>
    /// Account controller
    /// </summary>
    /// <remarks>
    /// https://docs.microsoft.com/en-us/aspnet/core/security/authentication/cookie?view=aspnetcore-2.2
    /// </remarks>
    /// <seealso cref="Controller" />
    [Route("account")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class AccountController : Controller
    {
        private readonly IApplicationConstants constants;
        private readonly UserManager<UserEntity> userManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountController"/> class.
        /// </summary>
        /// <param name="constants">The application constants.</param>
        /// <param name="userManager">The user manager.</param>
        public AccountController(IApplicationConstants constants, UserManager<UserEntity> userManager)
        {
            this.constants = constants ?? throw new ArgumentNullException(nameof(constants));
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        /// <summary>
        /// Renders the login page.
        /// </summary>
        /// <returns>The login page.</returns>
        [HttpGet("login")]
        public async Task<IActionResult> Login()
        {
            var result = await HttpContext.AuthenticateAsync("Cookies");
            if (result?.Succeeded ?? false)
            {
                HttpContext.User = result.Principal;
            }

            return View();
        }

        /// <summary>
        /// Renders the test page.
        /// </summary>
        /// <returns>The test page.</returns>
        [HttpGet("test")]
        [Authorize(AuthenticationSchemes = "Cookies")]
        public async Task<IActionResult> Test()
        {
            var test1 = HttpContext.User.Identity.IsAuthenticated;

            await HttpContext.AuthenticateAsync("Cookies");

            var test2 = HttpContext.User.Identity.IsAuthenticated;

            return View();
        }

        /// <summary>
        /// Posts the credentials to login.
        /// </summary>
        /// <param name="credentials">The credentials.</param>
        /// <returns>The login result.</returns>
        [HttpPost("login")]
        public async Task<IActionResult> PostLogin([FromBody]CredentialsViewModel credentials)
        {
            var user = await userManager.FindByNameAsync(credentials.Username);
            if (user == null)
            {
                throw new UnexpectedNullException("Incorrect username or password.");
            }

            bool isValid = await userManager.CheckPasswordAsync(user, credentials.Password);
            if (!isValid)
            {
                throw new UnexpectedNullException("Incorrect username or password.");
            }

            var model = UserModel.Create(user, constants);

            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, model.Id),
                    new Claim("username", model.Username),

                    // TODO
                    // new Claim("LastChanged", "Database Value...")
                };

            foreach (string role in model.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(2),
                IsPersistent = true,
                IssuedUtc = DateTimeOffset.UtcNow
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            return Ok();
        }

        /// <summary>
        /// Performs the logout.
        /// </summary>
        /// <returns>The logout result.</returns>
        [HttpPost("logout")]
        public async Task<IActionResult> PostLogout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction(nameof(Login));
        }
    }
}
