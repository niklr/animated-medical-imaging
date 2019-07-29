﻿namespace AMI.Core.Constants
{
    /// <summary>
    /// Constants of the application.
    /// </summary>
    public interface IApplicationConstants
    {
        /// <summary>
        /// Gets the short version of the application name.
        /// </summary>
        string ApplicationNameShort { get; }

        /// <summary>
        /// Gets the default file extension.
        /// </summary>
        string DefaultFileExtension { get; }

        /// <summary>
        /// Gets the default pagination limit.
        /// </summary>
        int DefaultPaginationLimit { get; }

        /// <summary>
        /// Gets the allowed values of the limit pagination parameter.
        /// </summary>
        int[] AllowedPaginationLimitValues { get; }

        /// <summary>
        /// Gets the character used to separate the role names.
        /// </summary>
        string RoleNameSeparator { get; }
    }
}
