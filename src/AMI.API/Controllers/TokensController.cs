using System;
using System.Net;
using System.Threading.Tasks;
using AMI.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Models = AMI.Core.Entities.Models;

namespace AMI.API.Controllers
{
    /// <summary>
    /// The endpoints related to tokens used for authentication and authorization purposes.
    /// </summary>
    [ApiController]
    [Route("tokens")]
    public class TokensController : BaseController
    {
        private readonly ITokenService tokenService;

        /// <summary>
        /// Initializes a new instance of the <see cref="TokensController"/> class.
        /// </summary>
        /// <param name="tokenService">The token service.</param>
        public TokensController(ITokenService tokenService)
        {
            this.tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
        }

        /// <summary>
        /// Create tokens
        /// </summary>
        /// <param name="credentials">The credentials.</param>
        /// <remarks>
        /// With this POST request you can obtain tokens with your credentials.
        /// Tokens are used for authentication and authorization purposes.
        /// </remarks>
        /// <returns>A model containing the tokens.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(Models.TokenContainerModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Create([FromBody] Models.CredentialsModel credentials)
        {
            return Ok(await tokenService.CreateAsync(credentials.Username, credentials.Password, CancellationToken));
        }

        /// <summary>
        /// Create anonymous tokens
        /// </summary>
        /// <remarks>
        /// With this POST request you can obtain tokens as anonymous user.
        /// Tokens are used for authentication and authorization purposes.
        /// </remarks>
        /// <returns>A model containing the tokens.</returns>
        [HttpPost("anon")]
        [ProducesResponseType(typeof(Models.TokenContainerModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateAnonymous()
        {
            return Ok(await tokenService.CreateAnonymousAsync(CancellationToken));
        }

        /// <summary>
        /// Update tokens
        /// </summary>
        /// <param name="container">The container with the tokens.</param>
        /// <remarks>
        /// With this PUT request you can update an expired access token with a new valid access token based on the provided refresh token.
        /// </remarks>
        /// <returns>A model containing the updated tokens.</returns>
        [HttpPut]
        [ProducesResponseType(typeof(Models.TokenContainerModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Update([FromBody] Models.TokenContainerModel container)
        {
            return Ok(await tokenService.UseRefreshTokenAsync(container.RefreshToken, CancellationToken));
        }
    }
}
