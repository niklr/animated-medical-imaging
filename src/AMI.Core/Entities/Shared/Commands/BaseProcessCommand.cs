using System;
using System.Collections.Generic;

using AMI.Domain.Enums;
using MediatR;

namespace AMI.Core.Entities.Shared.Commands
{
    /// <summary>
    /// A command containing information needed for processing.
    /// </summary>
    /// <typeparam name="T">The type of the response.</typeparam>
    [Serializable]
    public abstract class BaseProcessCommand<T> : BaseCommand, IRequest<T>
    {
        /// <summary>
        /// Gets or sets the desired output size of the processed images.
        /// </summary>
        public int OutputSize { get; set; }

        /// <summary>
        /// Gets or sets the amount of images per axis.
        /// </summary>
        public int AmountPerAxis { get; set; }

        /// <summary>
        /// Gets or sets the axis types to be considered.
        /// </summary>
        public ISet<AxisType> AxisTypes { get; set; } = new HashSet<AxisType>();

        /// <summary>
        /// Gets or sets the image format.
        /// </summary>
        public ImageFormat ImageFormat { get; set; } = ImageFormat.Png;

        /// <summary>
        /// Gets or sets the Bézier easing type per axis used for the animated image.
        /// </summary>
        public BezierEasingType BezierEasingTypePerAxis { get; set; } = BezierEasingType.Linear;

        /// <summary>
        /// Gets or sets the Bézier easing type used for the combined animated image.
        /// </summary>
        public BezierEasingType BezierEasingTypeCombined { get; set; } = BezierEasingType.Linear;

        /// <summary>
        /// Gets or sets a value indicating whether the images should be converted to grayscale.
        /// </summary>
        public bool Grayscale { get; set; } = true;
    }
}
