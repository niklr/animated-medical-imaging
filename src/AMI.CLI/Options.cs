using CommandLine;

namespace AMI.NetCore.Portable
{
    public class Options
    {
        [Option("DesiredSize", Required = false, HelpText = "The size of the output images in pixels. (Default is 250)")]
        public uint? DesiredSize { get; set; } = 250;

        [Option("AmountPerAxis", Required = false, HelpText = "The amount of images to be extracted per axis. (Default is 10)")]
        public uint AmountPerAxis { get; set; } = 10;

        [Option("SourcePath", Required = true, HelpText = "The path of the source image.")]
        public string SourcePath { get; set; }

        //public string WatermarkSourcePath { get; set; }

        [Option("DestinationPath", Required = true, HelpText = "The path of the destination where the output should be written.")]
        public string DestinationPath { get; set; }

        //public ISet<AxisType> AxisTypes { get; } = new HashSet<AxisType>();

        //public ImageFormat ImageFormat { get; set; } = ImageFormat.Png;

        //public BezierEasingType BezierEasingTypePerAxis { get; set; } = BezierEasingType.Linear;

        //public BezierEasingType BezierEasingTypeCombined { get; set; } = BezierEasingType.Linear;

        [Option("Grayscale", Required = false, HelpText = "Whether the images should be converted to grayscale. (Default is 1)")]
        public int Grayscale { get; set; } = 1;

        [Option("OpenCombinedGif", Required = false, HelpText = "Whether the combined gif should be opened after creation. (Default is 0)")]
        public int OpenCombinedGif { get; set; } = 0;
    }
}
