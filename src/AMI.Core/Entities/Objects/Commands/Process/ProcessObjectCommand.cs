using System;
using System.Collections.Generic;
using AMI.Core.Entities.Models;
using AMI.Domain.Enums;
using MediatR;

namespace AMI.Core.Entities.Objects.Commands.Process
{
    /// <summary>
    /// A command containing information needed for processing.
    /// </summary>
    [Serializable]
    public class ProcessObjectCommand : IRequest<ProcessResult>
    {
        /// <summary>
        /// Gets or sets the desired size of the processed images.
        /// </summary>
        public uint? DesiredSize { get; set; }

        /// <summary>
        /// Gets or sets the amount of images per axis.
        /// </summary>
        public uint AmountPerAxis { get; set; }

        /// <summary>
        /// Gets or sets the source path.
        /// </summary>
        public string SourcePath { get; set; }

        /// <summary>
        /// Gets or sets the source path of the watermark.
        /// </summary>
        public string WatermarkSourcePath { get; set; }

        /// <summary>
        /// Gets or sets the destination path.
        /// </summary>
        public string DestinationPath { get; set; }

        /// <summary>
        /// Gets the axis types to be considered.
        /// </summary>
        public ISet<AxisType> AxisTypes { get; } = new HashSet<AxisType>();

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

        /// <summary>
        /// Gets or sets a value indicating whether the combined animated image should be opened after the processing.
        /// </summary>
        public bool OpenCombinedGif { get; set; } = false;
    }
}
