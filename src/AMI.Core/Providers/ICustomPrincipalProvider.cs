using AMI.Core.Entities.Models;

namespace AMI.Core.Providers
{
    /// <summary>
    /// An interface representing a provider for custom principals.
    /// </summary>
    public interface ICustomPrincipalProvider
    {
        /// <summary>
        /// Gets the custom principal.
        /// </summary>
        /// <returns>The custom principal.</returns>
        ICustomPrincipal GetPrincipal();
    }
}
