namespace AMI.Core.IO.Builders
{
    /// <summary>
    /// An interface representing a builder for gateway group names.
    /// </summary>
    public interface IGatewayGroupNameBuilder
    {
        /// <summary>
        /// Builds the gateway group name for administrators.
        /// </summary>
        /// <returns>The gateway group name for administrators.</returns>
        string BuildAdministratorGroupName();

        /// <summary>
        /// Builds the gateway group name for the specified user identifier.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>The gateway group name for the specified user identifier.</returns>
        string BuildUserIdGroupName(string userId);
    }
}
