using AMI.Core.Enums;

namespace AMI.Core.Entities.Models
{
    /// <summary>
    /// A model containing information about the axis in the coordinate system.
    /// </summary>
    /// <typeparam name="T">The type of the entity associated with the axis.</typeparam>
    public class AxisContainer<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AxisContainer{T}"/> class.
        /// </summary>
        /// <param name="axisType">Type of the axis.</param>
        /// <param name="entity">The entity to associate with the axis.</param>
        public AxisContainer(AxisType axisType, T entity)
        {
            AxisType = axisType;
            Entity = entity;
        }

        /// <summary>
        /// Gets or sets the type of the axis.
        /// </summary>
        public AxisType AxisType { get; set; }

        /// <summary>
        /// Gets or sets the entity.
        /// </summary>
        public T Entity { get; set; }
    }
}
