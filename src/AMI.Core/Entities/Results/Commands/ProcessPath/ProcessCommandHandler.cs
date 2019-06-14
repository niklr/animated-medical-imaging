﻿using System;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Entities.Models;
using AMI.Core.Entities.Shared.Commands;
using AMI.Core.IO.Serializers;
using AMI.Core.Repositories;
using AMI.Core.Services;
using AMI.Domain.Entities;
using AMI.Domain.Enums;

namespace AMI.Core.Entities.Results.Commands.ProcessPath
{
    /// <summary>
    /// A handler for process command requests.
    /// </summary>
    public class ProcessCommandHandler : BaseCommandRequestHandler<ProcessPathCommand, ProcessResultModel>
    {
        private readonly IAmiUnitOfWork context;
        private readonly IIdGenService idGenService;
        private readonly IDefaultJsonSerializer serializer;
        private readonly IImageService imageService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessCommandHandler"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="idGenService">The service to generate unique identifiers.</param>
        /// <param name="serializer">The JSON serializer.</param>
        /// <param name="imageService">The image service.</param>
        public ProcessCommandHandler(
            IAmiUnitOfWork context,
            IIdGenService idGenService,
            IDefaultJsonSerializer serializer,
            IImageService imageService)
            : base()
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.idGenService = idGenService ?? throw new ArgumentNullException(nameof(idGenService));
            this.serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
            this.imageService = imageService ?? throw new ArgumentNullException(nameof(imageService));
        }

        /// <inheritdoc/>
        protected override async Task<ProcessResultModel> ProtectedHandleAsync(ProcessPathCommand request, CancellationToken cancellationToken)
        {
            var result = await imageService.ProcessAsync(request, cancellationToken);

            var entity = new ResultEntity()
            {
                Id = idGenService.CreateId(),
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow,
                Version = result.Version,
                JsonFsPath = result.JsonFilename,
                ResultType = (int)ResultType.ProcessResult,
                ResultSerialized = serializer.Serialize(result)
            };

            context.ResultRepository.Add(entity);

            await context.SaveChangesAsync(cancellationToken);

            return result;
        }
    }
}
