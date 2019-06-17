using System;
using AMI.Core.Entities.Models;
using AMI.Core.Entities.Shared.Commands;
using AMI.Domain.Enums;

namespace AMI.Core.Entities.Results.Commands.ProcessPath
{
    /// <summary>
    /// A command containing information needed to process paths (directory, file, url, etc.).
    /// </summary>
    [Serializable]
    public class ProcessPathCommand : BaseProcessCommand<ProcessResultModel>
    {
        /// <inheritdoc/>
        public override CommandType CommandType => CommandType.ProcessPathCommand;

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
    }
}
