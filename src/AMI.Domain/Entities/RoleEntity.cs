using System;

namespace AMI.Domain.Entities
{
    /// <summary>
    /// An entity representing a role.
    /// </summary>
    public class RoleEntity
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the normalized name.
        /// </summary>
        public string NormalizedName { get; set; }

    }
}
