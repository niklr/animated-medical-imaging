﻿namespace AMI.Domain.Enums
{
    /// <summary>
    /// A type to describe the role related to authentication and authorization.
    /// </summary>
    public enum RoleType
    {
        /// <summary>
        /// The type of the role is not known.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// The user role.
        /// </summary>
        User = 1,

        /// <summary>
        /// The administrator role.
        /// </summary>
        Administrator = 2,

        /// <summary>
        /// The service role.
        /// </summary>
        Service = 3
    }
}
