using System;
using System.Linq.Expressions;
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
        /// Gets the projection of the domain entity.
        /// </summary>
        public static Expression<Func<ObjectVersion, ObjectModel>> Projection
        {
            get
            {
                return e => new ObjectModel
                {
                    Id = e.Id.ToString()
                };
            }
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
        public static ObjectModel Create(ObjectVersion entity)
        {
            if (entity == null)
            {
                return null;
            }

            return Projection.Compile().Invoke(entity);
        }
    }
}
