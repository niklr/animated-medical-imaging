using AMI.Core.Enums;

namespace AMI.Core.Entities.Models
{
    /// <summary>
    /// A model containing information about the position.
    /// </summary>
    /// <typeparam name="T">The type of the entity associated with the axis.</typeparam>
    /// <seealso cref="AxisContainer{T}" />
    public class PositionAxisContainer<T> : AxisContainer<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PositionAxisContainer{T}"/> class.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="axisType">Type of the axis.</param>
        /// <param name="entity">The entity to associate with the axis.</param>
        public PositionAxisContainer(uint position, AxisType axisType, T entity)
            : base(axisType, entity)
        {
            Position = position;
        }

        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        public uint Position { get; set; }
    }
}
