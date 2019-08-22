using AMI.Domain.Enums;

namespace AMI.Core.Mappers
{
    /// <summary>
    /// A mapper for positions of each axis in the coordinate system.
    /// </summary>
    public interface IAxisPositionMapper
    {
        /// <summary>Calculates the mapped position (default is 0).</summary>
        /// <param name="amount">The amount of images.</param>
        /// <param name="length">The length of the current axis.</param>
        /// <param name="position">The current position.</param>
        /// <returns>The mapped position (default is 0).</returns>
        int CalculateMappedPosition(int amount, int length, int position);

        /// <summary>
        /// Gets the length based on the specified axis type.
        /// </summary>
        /// <param name="axisType">Type of the axis.</param>
        /// <returns>The length of the axis.</returns>
        int GetLength(AxisType axisType);

        /// <summary>
        /// Gets the mapped position.
        /// </summary>
        /// <param name="axisType">Type of the axis.</param>
        /// <param name="position">The position.</param>
        /// <returns>The mapped position.</returns>
        int GetMappedPosition(AxisType axisType, int position);
    }
}
