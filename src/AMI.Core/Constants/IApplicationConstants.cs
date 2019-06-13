namespace AMI.Core.Constants
{
    /// <summary>
    /// Constants of the application.
    /// </summary>
    public interface IApplicationConstants
    {
        /// <summary>
        /// Gets the default file extension.
        /// </summary>
        string DefaultFileExtension { get; }

        /// <summary>
        /// Gets the allowed values of the limit pagination parameter.
        /// </summary>
        int[] AllowedPaginationLimitValues { get; }
    }
}
