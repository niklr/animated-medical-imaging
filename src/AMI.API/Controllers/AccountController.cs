using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AMI.API.ViewModels;
using AMI.Core.Constants;
using AMI.Core.Entities.Models;
using AMI.Core.Services;
using AMI.Domain.Entities;
using AMI.Domain.Exceptions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RNS.Framework.Tools;

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
    public class AccountController : BaseController
    {
        private readonly IApplicationConstants constants;
        private readonly UserManager<UserEntity> userManager;
        private readonly ITokenService tokenService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountController"/> class.
        /// </summary>
        /// <param name="constants">The application constants.</param>
        /// <param name="userManager">The user manager.</param>
        /// <param name="tokenService">The token service.</param>
        public AccountController(IApplicationConstants constants, UserManager<UserEntity> userManager, ITokenService tokenService)
        {
            this.constants = constants ?? throw new ArgumentNullException(nameof(constants));
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            this.tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
        }

        /// <summary>
        /// Renders the login page.
        /// </summary>
        /// <param name="token">The encoded access token.</param>
        /// <param name="redirectUrl">The URL redirected to after authentication was successful.</param>
        /// <returns>The login page.</returns>
        [HttpGet("login")]
        public async Task<IActionResult> Login(string token, string redirectUrl)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                var result = await HttpContext.AuthenticateAsync("Cookies");
                if (result?.Succeeded ?? false)
                {
                    HttpContext.User = result.Principal;
                }
            }
            else
            {
                var decodedToken = await tokenService.DecodeAsync<AccessTokenModel>(token, CancellationToken);
                if (decodedToken == null)
                {
                    throw new UnexpectedNullException("Incorrect token.");
                }

                var user = await userManager.FindByNameAsync(decodedToken.Username);
                if (user == null)
                {
                    throw new UnexpectedNullException("Incorrect token.");
                }

                await SignInAsync(user);
            }

            if (!string.IsNullOrWhiteSpace(redirectUrl) && redirectUrl.StartsWith(AppBaseUrl))
            {
                return Redirect(redirectUrl);
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

            await SignInAsync(user);

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

        private async Task SignInAsync(UserEntity user)
        {
            Ensure.ArgumentNotNull(user, nameof(user));

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
        }
    }
}
