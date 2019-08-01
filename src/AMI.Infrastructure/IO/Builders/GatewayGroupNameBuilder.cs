using AMI.Core.IO.Builders;

namespace AMI.Infrastructure.IO.Builders
{
    /// <summary>
    /// The builder for gatway group names.
    /// </summary>
    public class GatewayGroupNameBuilder : IGatewayGroupNameBuilder
    {
        /// <inheritdoc/>
        public string BuildAdministratorGroupName()
        {
            return "AdministratorGatewayGroup";
        }

        /// <inheritdoc/>
        public string BuildUserIdGroupName(string userId)
        {
            return $"UserIdGatewayGroup_{userId}";
        }
    }
}
