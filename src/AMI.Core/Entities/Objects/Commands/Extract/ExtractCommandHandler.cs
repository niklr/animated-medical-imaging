using System;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Entities.Models;
using AMI.Core.Entities.Shared.Commands;
using AMI.Core.Services;
using MediatR;

namespace AMI.Core.Entities.Objects.Commands.Extract
{
    /// <summary>
    /// A handler for extract command requests.
    /// </summary>
    /// <seealso cref="BaseCommandRequestHandler{ExtractObjectCommand, ExtractResult}" />
    public class ExtractCommandHandler : BaseCommandRequestHandler<ExtractObjectCommand, ExtractResult>
    {
        private readonly IImageService imageService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtractCommandHandler"/> class.
        /// </summary>
        /// <param name="mediator">The mediator.</param>
        /// <param name="imageService">The image service.</param>
        /// <exception cref="ArgumentNullException">imageService</exception>
        public ExtractCommandHandler(IMediator mediator, IImageService imageService)
            : base(mediator)
        {
            this.imageService = imageService ?? throw new ArgumentNullException(nameof(imageService));
        }

        /// <summary>
        /// Handles the extract command request called by the base class.
        /// </summary>
        /// <param name="request">The command request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The result of the extract command request.</returns>
        protected override Task<ExtractResult> ProtectedHandle(ExtractObjectCommand request, CancellationToken cancellationToken)
        {
            return imageService.ExtractAsync(request, cancellationToken);
        }
    }
}
