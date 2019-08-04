using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using AMI.Core.Entities.Models;
using AMI.Domain.Exceptions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using RNS.Framework.Tools;

namespace AMI.API.Extensions.ServiceCollectionExtensions
{
    /// <summary>
    /// Extensions related to <see cref="IServiceCollection"/>
    /// </summary>
    public static class CustomAuthenticationExtensions
    {
        /// <summary>
        /// Extension method used to add the custom authentication.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="environment">The hosting environment.</param>
        /// <param name="configuration">The configuration.</param>
        public static void AddCustomAuthentication(this IServiceCollection services, IHostingEnvironment environment, IConfiguration configuration)
        {
            Ensure.ArgumentNotNull(services, nameof(services));
            Ensure.ArgumentNotNull(environment, nameof(environment));
            Ensure.ArgumentNotNull(configuration, nameof(configuration));

            var jwtOptions = new AuthJwtOptions();
            configuration.GetSection("ApiOptions:AuthOptions:JwtOptions").Bind(jwtOptions);
            string exceptionMessage = "ApiOptions:AuthOptions:JwtOptions:{0} is missing.";

            if (string.IsNullOrWhiteSpace(jwtOptions.SecretKey))
            {
                throw new UnexpectedNullException(string.Format(exceptionMessage, nameof(jwtOptions.SecretKey)));
            }

            if (string.IsNullOrWhiteSpace(jwtOptions.Issuer))
            {
                throw new UnexpectedNullException(string.Format(exceptionMessage, nameof(jwtOptions.Issuer)));
            }

            if (string.IsNullOrWhiteSpace(jwtOptions.Audience))
            {
                throw new UnexpectedNullException(string.Format(exceptionMessage, nameof(jwtOptions.Audience)));
            }

            if (string.IsNullOrWhiteSpace(jwtOptions.NameClaimType))
            {
                throw new UnexpectedNullException(string.Format(exceptionMessage, nameof(jwtOptions.NameClaimType)));
            }

            if (string.IsNullOrWhiteSpace(jwtOptions.RoleClaimType))
            {
                throw new UnexpectedNullException(string.Format(exceptionMessage, nameof(jwtOptions.RoleClaimType)));
            }

            if (environment.IsDevelopment())
            {
                IdentityModelEventSource.ShowPII = true;
            }

            // Avoid System.ArgumentOutOfRangeException: IDX10603: Decryption failed. Keys tried: 'HS256'.
            if (jwtOptions.SecretKey.Length < 16)
            {
                throw new AmiException("JWT secret key must consist of minimum 16 characters.");
            }

            services.AddAuthentication(o =>
            {
                o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                o.RequireHttpsMetadata = false;
                o.Audience = jwtOptions.Audience;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtOptions.SecretKey)),

                    ValidateIssuer = true,
                    ValidIssuer = jwtOptions.Issuer,

                    ValidateAudience = true,
                    ValidAudience = jwtOptions.Audience,

                    ValidateActor = true,
                    ValidateLifetime = true,

                    NameClaimType = jwtOptions.NameClaimType,
                    RoleClaimType = jwtOptions.RoleClaimType
                };
                o.Events = new JwtBearerEvents()
                {
                    OnMessageReceived = (context) =>
                    {
                        // pull the bearer token out of the QueryString for WebSocket connections
                        if (context.Request.Query.TryGetValue("authtoken", out StringValues token))
                        {
                            Debug.WriteLine($"authtoken received: {token}");
                            context.Token = token;
                        }

                        return Task.CompletedTask;
                    },
                    OnAuthenticationFailed = c =>
                    {
                        if (c.Exception is AuthException)
                        {
                            return Task.CompletedTask;
                        }
                        else
                        {
                            throw new AuthException(c.Exception.Message);
                        }
                    }
                };
            });
        }
    }
}
