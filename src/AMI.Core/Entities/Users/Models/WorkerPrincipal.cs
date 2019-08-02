using System;
using System.Security.Principal;
using AMI.Domain.Enums;

namespace AMI.Core.Entities.Models
{
    /// <summary>
    /// A principal intended to use in background workers.
    /// </summary>
    /// <seealso cref="ICustomPrincipal" />
    public class WorkerPrincipal : ICustomPrincipal
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WorkerPrincipal"/> class.
        /// </summary>
        public WorkerPrincipal()
        {
            Identity = new CustomIdentity()
            {
                Name = new Guid("11111111-1111-1111-1111-111111111111").ToString(),
                Username = "worker"
            };
        }

        /// <inheritdoc/>
        public ICustomIdentity Identity { get; }

        IIdentity IPrincipal.Identity => Identity;

        /// <inheritdoc/>
        public bool IsInRole(RoleType role)
        {
            return true;
        }

        /// <inheritdoc/>
        public bool IsInRole(string role)
        {
            return true;
        }
    }
}
