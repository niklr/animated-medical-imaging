using System;
using AMI.Core.Entities.Shared.Models;
using AMI.Domain.Entities;
using AMI.Domain.Enums;

namespace AMI.Core.Entities.Models
{
    /// <summary>
    /// A model representing an object.
    /// </summary>
    public class ObjectModel : IEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectModel"/> class.
        /// </summary>
        public ObjectModel()
        {
        }

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
        /// Gets or sets the type of the data.
        /// </summary>
        public DataType DataType { get; set; } = DataType.Unknown;

        /// <summary>
        /// Gets or sets the file format.
        /// </summary>
        public FileFormat FileFormat { get; set; } = FileFormat.Unknown;

        /// <summary>
        /// Gets or sets the original filename.
        /// </summary>
        public string OriginalFilename { get; set; }

        /// <summary>
        /// Gets or sets the source path (directory, file, url, etc.).
        /// </summary>
        public string SourcePath { get; set; }

        /// <summary>
        /// Gets or sets the uncompressed filesystem path (directory).
        /// </summary>
        public string UncompressedFilesystemPath { get; set; }

        /// <summary>
        /// Creates a model based on the given domain entity.
        /// </summary>
        /// <param name="entity">The domain entity.</param>
        /// <returns>The domain entity as a model.</returns>
        public static ObjectModel Create(ObjectEntity entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new ObjectModel
            {
                Id = entity.Id.ToString(),
                CreatedDate = entity.CreatedDate,
                ModifiedDate = entity.ModifiedDate,
                DataType = Enum.TryParse(entity.DataType.ToString(), out DataType dataType) ? dataType : DataType.Unknown,
                FileFormat = Enum.TryParse(entity.FileFormat.ToString(), out FileFormat fileFormat) ? fileFormat : FileFormat.Unknown,
                OriginalFilename = entity.OriginalFilename,
                SourcePath = entity.SourcePath,
                UncompressedFilesystemPath = entity.UncompressedFilesystemPath
            };
        }
    }
}
