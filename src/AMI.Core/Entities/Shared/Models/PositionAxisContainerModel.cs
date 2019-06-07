using AMI.Domain.Enums;

namespace AMI.Core.Entities.Models
{
    /// <summary>
    /// A model containing information about the position.
    /// </summary>
    /// <typeparam name="T">The type of the entity associated with the axis.</typeparam>
    /// <seealso cref="AxisContainerModel{T}" />
    public class PositionAxisContainerModel<T> : AxisContainerModel<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PositionAxisContainerModel{T}"/> class.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="axisType">Type of the axis.</param>
        /// <param name="entity">The entity to associate with the axis.</param>
        public PositionAxisContainerModel(uint position, AxisType axisType, T entity)
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
