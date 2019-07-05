namespace AMI.Core.Configurations
{
    /// <summary>
    /// An interface representing the base configuration.
    /// </summary>
    /// <typeparam name="T">The type of options being requested.</typeparam>
    public interface IBaseConfiguration<T>
    {
        /// <summary>
        /// Clones the configuration options.
        /// </summary>
        /// <returns>The configuration options.</returns>
        T Clone();
    }
}
