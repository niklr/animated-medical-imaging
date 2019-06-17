using AMI.Domain.Enums;

namespace AMI.Core.Entities.Shared.Commands
{
    /// <summary>
    /// An interface for the base command.
    /// </summary>
    public interface IBaseCommand
    {
        /// <summary>
        /// Gets the type of the command.
        /// </summary>
        CommandType CommandType { get; }
    }
}
