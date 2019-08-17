using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Configurations;
using AMI.Core.Entities.Models;
using AMI.Core.IO.Generators;
using AMI.Core.Providers;
using AMI.Core.Services;
using AMI.Domain.Entities;
using AMI.Domain.Enums;
using AMI.Domain.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using RNS.Framework.Extensions.Reflection;
using RNS.Framework.Tools;

namespace AMI.Infrastructure.Services
{
    /// <summary>
    /// A service for identities related to security.
    /// </summary>
    public class IdentityService : IIdentityService
    {
        private readonly ILogger logger;
        private readonly IApiConfiguration configuration;
        private readonly ICustomPrincipalProvider principalProvider;
        private readonly UserManager<UserEntity> userManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="IdentityService"/> class.
        /// </summary>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="configuration">The API configuration.</param>
        /// <param name="idGenerator">The generator for unique identifiers.</param>
        /// <param name="principalProvider">The principal provider.</param>
        /// <param name="userManager">The user manager.</param>
        public IdentityService(
            ILoggerFactory loggerFactory,
            IApiConfiguration configuration,
            IIdGenerator idGenerator,
            ICustomPrincipalProvider principalProvider,
            UserManager<UserEntity> userManager)
        {
            logger = loggerFactory?.CreateLogger<IdentityService>() ?? throw new ArgumentNullException(nameof(loggerFactory));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.principalProvider = principalProvider ?? throw new ArgumentNullException(nameof(principalProvider));
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        /// <inheritdoc/>
        public async Task EnsureUsersExistAsync(CancellationToken ct)
        {
            Ensure.ArgumentNotNull(ct, nameof(ct));

            logger.LogInformation($"{this.GetMethodName()} started");

            var defaultPassword = "123456";
            var passwords = configuration.Options?.AuthOptions?.UserPasswords;

            var entities = new List<IAuthEntity>()
            {
                new AuthEntity()
                {
                    Username = "Svc",
                    Password = string.IsNullOrWhiteSpace(passwords?.Svc) ? defaultPassword : passwords.Svc,
                    Roles = new List<string>() { RoleType.User.ToString() }
                },
                new AuthEntity()
                {
                    Username = "Admin",
                    Password = string.IsNullOrWhiteSpace(passwords?.Admin) ? defaultPassword : passwords.Admin,
                    Roles = new List<string>() { RoleType.User.ToString(), RoleType.Administrator.ToString() }
                }
            };

            var exceptions = new List<Exception>();

            foreach (var identity in entities)
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
                        if (result.Succeeded)
                        {
                            var userEntity = await userManager.FindByNameAsync(identity.Username);
                            if (userEntity == null)
                            {
                                throw new UnexpectedNullException("User not found after creation.");
                            }

                            if (identity.Roles?.Count > 0)
                            {
                                await userManager.AddToRolesAsync(userEntity, identity.Roles);
                            }
                        }
                        else
                        {
                            throw new AmiException(string.Join(" ", result.Errors.Select(x => x.Description)));
                        }
                    }
                    else
                    {
                        // Reset password
                        await userManager.RemovePasswordAsync(existing);
                        await userManager.AddPasswordAsync(existing, identity.Password);
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

            logger.LogInformation($"{this.GetMethodName()} ended");
        }

        /// <inheritdoc/>
        public bool IsAuthorized(string ownerId)
        {
            try
            {
                Ensure.ArgumentNotNullOrWhiteSpace(ownerId, nameof(ownerId));

                var principal = principalProvider.GetPrincipal();
                if (principal == null)
                {
                    throw new UnexpectedNullException("Principal could not be retrieved.");
                }

                if (ownerId.Equals(principal.Identity.Name) || principal.IsInRole(RoleType.Administrator))
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
            }

            return false;
        }
    }
}
