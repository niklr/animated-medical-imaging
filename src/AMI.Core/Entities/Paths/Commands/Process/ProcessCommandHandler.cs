﻿using System;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Entities.Models;
using AMI.Core.Entities.Shared.Commands;
using AMI.Core.Services;
using MediatR;

namespace AMI.Core.Entities.Paths.Commands.Process
{
    /// <summary>
    /// A handler for process command requests.
    /// </summary>
    /// <seealso cref="BaseCommandRequestHandler{ProcessPathCommand, ProcessResultModel}" />
    public class ProcessCommandHandler : BaseCommandRequestHandler<ProcessPathCommand, ProcessResultModel>
    {
        private readonly IImageService imageService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessCommandHandler"/> class.
        /// </summary>
        /// <param name="mediator">The mediator.</param>
        /// <param name="imageService">The image service.</param>
        /// <exception cref="ArgumentNullException">imageService</exception>
        public ProcessCommandHandler(IMediator mediator, IImageService imageService)
            : base(mediator)
        {
            this.imageService = imageService ?? throw new ArgumentNullException(nameof(imageService));
        }

        /// <inheritdoc/>
        protected override Task<ProcessResultModel> ProtectedHandle(ProcessPathCommand request, CancellationToken cancellationToken)
        {
            return imageService.ProcessAsync(request, cancellationToken);
        }
    }
}
