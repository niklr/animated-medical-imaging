﻿using System;
using AMI.Core.Entities.Models;
using AMI.Core.Entities.Shared.Commands;

namespace AMI.Core.Entities.Results.Commands.ProcessObject
{
    /// <summary>
    /// A command containing information needed to process objects.
    /// </summary>
    [Serializable]
    public class ProcessObjectCommand : BaseProcessCommand<ProcessResultModel>
    {
        /// <summary>
        /// Gets or sets the identifier of the object.
        /// </summary>
        public string Id { get; set; }
    }
}
