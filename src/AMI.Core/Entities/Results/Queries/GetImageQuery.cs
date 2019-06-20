﻿using AMI.Core.Entities.Models;
using MediatR;

namespace AMI.Core.Entities.Results.Queries
{
    /// <summary>
    /// A query to get the image file result.
    /// </summary>
    public class GetImageQuery : IRequest<FileResultModel>
    {
        /// <summary>
        /// Gets or sets the identifier of the result.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the filename of the image.
        /// </summary>
        public string Filename { get; set; }
    }
}
