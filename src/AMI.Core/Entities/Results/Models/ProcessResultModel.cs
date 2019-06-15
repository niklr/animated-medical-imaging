using System;
using System.Collections.Generic;
using AMI.Core.IO.Serializers;
using AMI.Domain.Entities;
using AMI.Domain.Enums;

namespace AMI.Core.Entities.Models
{
    /// <summary>
    /// A model containing information about the result of the image processing.
    /// </summary>
    public class ProcessResultModel : ResultModel
    {
        private IReadOnlyList<PositionAxisContainerModel<string>> images = new List<PositionAxisContainerModel<string>>();
        private IReadOnlyList<AxisContainerModel<string>> gifs = new List<AxisContainerModel<string>>();

        /// <inheritdoc/>
        public override ResultType ResultType
        {
            get
            {
                return ResultType.ProcessResult;
            }
        }

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

        /// <summary>
        /// Creates a model based on the given domain entity.
        /// </summary>
        /// <param name="entity">The domain entity.</param>
        /// <param name="serializer">The JSON serializer.</param>
        /// <returns>The domain entity as a model.</returns>
        public static new ProcessResultModel Create(ResultEntity entity, IDefaultJsonSerializer serializer)
        {
            if (entity == null)
            {
                return null;
            }

            var model = new ProcessResultModel
            {
                Id = entity.Id.ToString(),
                CreatedDate = entity.CreatedDate,
                ModifiedDate = entity.ModifiedDate,
                Version = entity.Version,
                JsonFilename = entity.JsonFilename
            };

            var deserialized = serializer.Deserialize<ProcessResultModel>(entity.ResultSerialized);
            if (deserialized != null)
            {
                model.Version = deserialized.Version;
                model.JsonFilename = deserialized.JsonFilename;
                model.LabelCount = deserialized.LabelCount;
                model.Images = deserialized.Images;
                model.Gifs = deserialized.Gifs;
                model.CombinedGif = deserialized.CombinedGif;
            }

            return model;
        }
    }
}
