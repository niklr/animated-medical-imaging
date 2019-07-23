using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Configurations;
using AMI.Core.IO.Generators;
using AMI.Core.Services;
using AMI.Domain.Entities;
using AMI.Domain.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace AMI.Infrastructure.Services
{
    /// <summary>
    /// A service for identities related to security.
    /// </summary>
    public class IdentityService : IIdentityService
    {
        private readonly ILogger logger;
        private readonly IApiConfiguration configuration;
        private readonly UserManager<UserEntity> userManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="IdentityService"/> class.
        /// </summary>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="configuration">The API configuration.</param>
        /// <param name="idGenerator">The generator for unique identifiers.</param>
        /// <param name="userManager">The user manager.</param>
        public IdentityService(
            ILoggerFactory loggerFactory,
            IApiConfiguration configuration,
            IIdGenerator idGenerator,
            UserManager<UserEntity> userManager)
        {
            logger = loggerFactory?.CreateLogger<ImageService>() ?? throw new ArgumentNullException(nameof(loggerFactory));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        /// <inheritdoc/>
        public async Task EnsureUsersExistAsync(CancellationToken ct)
        {
            if (ct == null)
            {
                throw new ArgumentNullException(nameof(ct));
            }

            var exceptions = new List<Exception>();

            foreach (var identity in configuration.Options.AuthOptions.Entities)
            {
                ct.ThrowIfCancellationRequested();

                try
                {
                    var existing = await userManager.FindByNameAsync(identity.Username);
                    if (existing == null)
                    {
                        var user = new UserEntity()
                        {
                            Username = identity.Username,
                            Email = $"{identity.Username}@localhost"
                        };
                        var result = await userManager.CreateAsync(user, identity.Password);
                        if (!result.Succeeded)
                        {
                            throw new AmiException(string.Join(" ", result.Errors.Select(x => x.Description)));
                        }
                    }
                }
                catch (Exception e)
                {
                    exceptions.Add(e);
                }
            }

            if (exceptions.Count > 0)
            {
                throw new AggregateException(exceptions);
            }
        }
    }
}
