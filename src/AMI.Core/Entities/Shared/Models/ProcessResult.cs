using System.Collections.Generic;

namespace AMI.Core.Entities.Models
{
    /// <summary>
    /// A model containing information about the result of the processing.
    /// </summary>
    public class ProcessResult
    {
        private IReadOnlyList<PositionAxisContainer<string>> images = new List<PositionAxisContainer<string>>();
        private IReadOnlyList<AxisContainer<string>> gifs = new List<AxisContainer<string>>();

        /// <summary>
        /// Gets or sets the application version.
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets the label count.
        /// </summary>
        public int LabelCount { get; set; }

        /// <summary>
        /// Gets or sets the position axis containers of the images.
        /// </summary>
        public IReadOnlyList<PositionAxisContainer<string>> Images
        {
            get
            {
                return images;
            }

            set
            {
                if (value != null)
                {
                    images = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the axis containers of the GIF images.
        /// </summary>
        public IReadOnlyList<AxisContainer<string>> Gifs
        {
            get
            {
                return gifs;
            }

            set
            {
                if (value != null)
                {
                    gifs = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the combined GIF.
        /// </summary>
        public string CombinedGif { get; set; }

        /// <summary>
        /// Gets or sets the JSON filename.
        /// </summary>
        public string JsonFilename { get; set; }
    }
}
