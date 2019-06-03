using System.Collections.Generic;

namespace AMI.Core.Entities.Models
{
    /// <summary>
    /// A model containing information about the ouput of the image processing.
    /// </summary>
    public class ImageProcessResult
    {
        private IReadOnlyList<PositionAxisContainer<string>> images = new List<PositionAxisContainer<string>>();

        /// <summary>
        /// Gets or sets the label count.
        /// </summary>
        public ulong LabelCount { get; set; }

        /// <summary>
        /// Gets or sets the images.
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
    }
}
