using AMI.Domain.Enums;

namespace AMI.Core.Mappers
{
    /// <summary>
    /// A mapper for positions of each axis in the coordinate system.
    /// </summary>
    public interface IAxisPositionMapper
    {
        /// <summary>Calculates the mapped position (default is 0).</summary>
        /// <param name="amount">The amount.</param>
        /// <param name="length">The length.</param>
        /// <param name="position">The position.</param>
        /// <returns>The mapped position (default is 0).</returns>
        uint CalculateMappedPosition(uint amount, uint length, uint position);

        /// <summary>
        /// Gets the mapped position.
        /// </summary>
        /// <param name="axisType">Type of the axis.</param>
        /// <param name="position">The position.</param>
        /// <returns>The mapped position.</returns>
        uint GetMappedPosition(AxisType axisType, uint position);
    }
}
