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
        /// Gets or sets the delay in milliseconds between frames of the animated sequence.
        /// Must be between 1 and 100. Default is 33.
        /// </summary>
        public int Delay { get; set; } = 33;

        /// <summary>
        /// Gets or sets the axis types to be considered.
        /// </summary>
        public ISet<AxisType> AxisTypes { get; set; } = new HashSet<AxisType>();

        /// <summary>
        /// Gets or sets the image format.
        /// Default is PNG.
        /// </summary>
        public ImageFormat ImageFormat { get; set; } = ImageFormat.Png;

        /// <summary>
        /// Gets or sets the Bézier easing type per axis used for the animated image.
        /// Default is linear.
        /// </summary>
        public BezierEasingType BezierEasingTypePerAxis { get; set; } = BezierEasingType.Linear;

        /// <summary>
        /// Gets or sets the Bézier easing type used for the combined animated image.
        /// Default is linear.
        /// </summary>
        public BezierEasingType BezierEasingTypeCombined { get; set; } = BezierEasingType.Linear;

        /// <summary>
        /// Gets or sets a value indicating whether the images should be converted to grayscale.
        /// Default is true.
        /// </summary>
        public bool Grayscale { get; set; } = true;
    }
}
