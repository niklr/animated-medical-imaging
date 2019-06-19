using CommandLine;

namespace AMI.Portable
{
    /// <summary>
    /// The options that can be provided to the command-line interface.
    /// </summary>
    public class Options
    {
        /// <summary>
        /// Gets or sets the size of the output images in pixels. (Default is 250)
        /// </summary>
        [Option("DesiredSize", Required = false, HelpText = "The size of the output images in pixels. (Default is 250)")]
        public int DesiredSize { get; set; } = 250;

        /// <summary>
        /// Gets or sets the amount of images to be extracted per axis. (Default is 10)
        /// </summary>
        [Option("AmountPerAxis", Required = false, HelpText = "The amount of images to be extracted per axis. (Default is 10)")]
        public int AmountPerAxis { get; set; } = 10;

        /// <summary>
        /// Gets or sets the source path of the image.
        /// </summary>
        [Option("SourcePath", Required = true, HelpText = "The path of the source image.")]
        public string SourcePath { get; set; }

        //public string WatermarkSourcePath { get; set; }

        /// <summary>
        /// Gets or sets the destination path where the output should be written.
        /// </summary>
        [Option("DestinationPath", Required = true, HelpText = "The path of the destination where the output should be written.")]
        public string DestinationPath { get; set; }

        //public ISet<AxisType> AxisTypes { get; } = new HashSet<AxisType>();

        //public ImageFormat ImageFormat { get; set; } = ImageFormat.Png;

        //public BezierEasingType BezierEasingTypePerAxis { get; set; } = BezierEasingType.Linear;

        //public BezierEasingType BezierEasingTypeCombined { get; set; } = BezierEasingType.Linear;

        /// <summary>
        /// Gets or sets a value indicating whether the images should be converted to grayscale. (Default is 1)
        /// </summary>
        [Option("Grayscale", Required = false, HelpText = "Whether the images should be converted to grayscale. (Default is 1)")]
        public int Grayscale { get; set; } = 1;

        /// <summary>
        /// Gets or sets a value indicating whether the combined gif should be opened after creation. (Default is 0)
        /// </summary>
        [Option("OpenCombinedGif", Required = false, HelpText = "Whether the combined gif should be opened after creation. (Default is 0)")]
        public int OpenCombinedGif { get; set; } = 0;
    }
}
