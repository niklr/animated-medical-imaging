namespace AMI.Core.IO.Builders
{
    /// <summary>
    /// An interface representing a builder for gateway group names.
    /// </summary>
    public interface IGatewayGroupNameBuilder
    {
        /// <summary>
        /// Builds the default name of the gateway group.
        /// </summary>
        /// <returns>The name of the default gateway group.</returns>
        string BuildDefaultGroupName();
    }
}
