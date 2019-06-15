using System;
using AMI.Core.IO.Serializers;
using AMI.Domain.Entities;
using AMI.Domain.Enums;

namespace AMI.Core.Entities.Models
{
    /// <summary>
    /// A model containing information about the result of the processing.
    /// </summary>
    public abstract class ResultModel
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the modified date.
        /// </summary>
        public DateTime ModifiedDate { get; set; }

        /// <summary>
        /// Gets the type of the result.
        /// </summary>
        public virtual ResultType ResultType
        {
            get
            {
                return ResultType.Unknown;
            }
        }

        /// <summary>
        /// Gets or sets the application version.
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets the JSON filename.
        /// </summary>
        public string JsonFilename { get; set; }

        /// <summary>
        /// Creates a model based on the given domain entity.
        /// </summary>
        /// <param name="entity">The domain entity.</param>
        /// <param name="serializer">The JSON serializer.</param>
        /// <returns>The domain entity as a model.</returns>
        public static ResultModel Create(ResultEntity entity, IDefaultJsonSerializer serializer)
        {
            if (entity == null)
            {
                return null;
            }

            if (Enum.TryParse(entity.ResultType.ToString(), out ResultType resultType))
            {
                switch (resultType)
                {
                    case ResultType.ProcessResult:
                        return ProcessResultModel.Create(entity, serializer);
                    default:
                        break;
                }
            }

            return null;
        }
    }
}
