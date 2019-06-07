using System.Collections.Generic;

namespace AMI.Core.Entities.Models
{
    /// <summary>
    /// A model containing information about the result of the image processing.
    /// </summary>
    public class ProcessResultModel : ResultModel
    {
        private IReadOnlyList<PositionAxisContainerModel<string>> images = new List<PositionAxisContainerModel<string>>();
        private IReadOnlyList<AxisContainerModel<string>> gifs = new List<AxisContainerModel<string>>();

        /// <summary>
        /// Gets or sets the label count.
        /// </summary>
        public int LabelCount { get; set; }

        /// <summary>
        /// Gets or sets the images.
        /// </summary>
        public IReadOnlyList<PositionAxisContainerModel<string>> Images
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
        public IReadOnlyList<AxisContainerModel<string>> Gifs
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
    }
}
