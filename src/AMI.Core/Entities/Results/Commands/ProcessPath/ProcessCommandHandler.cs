﻿using System;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Entities.Models;
using AMI.Core.Entities.Shared.Commands;
using AMI.Core.IO.Generators;
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
        private readonly IIdGenerator idGenerator;
        private readonly IDefaultJsonSerializer serializer;
        private readonly IImageService imageService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessCommandHandler"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="gateway">The gateway service.</param>
        /// <param name="idGenerator">The generator for unique identifiers.</param>
        /// <param name="serializer">The JSON serializer.</param>
        /// <param name="imageService">The image service.</param>
        public ProcessCommandHandler(
            IAmiUnitOfWork context,
            IGatewayService gateway,
            IIdGenerator idGenerator,
            IDefaultJsonSerializer serializer,
            IImageService imageService)
            : base(context, gateway)
        {
            this.idGenerator = idGenerator ?? throw new ArgumentNullException(nameof(idGenerator));
            this.serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
            this.imageService = imageService ?? throw new ArgumentNullException(nameof(imageService));
        }

        /// <inheritdoc/>
        protected override async Task<ProcessResultModel> ProtectedHandleAsync(ProcessPathCommand request, CancellationToken cancellationToken)
        {
            var result = await imageService.ProcessAsync(request, cancellationToken);

            var entity = new ResultEntity()
            {
                Id = idGenerator.GenerateId(),
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow,
                Version = result.Version,
                JsonFilename = result.JsonFilename,
                ResultType = (int)ResultType.ProcessResult,
                ResultSerialized = serializer.Serialize(result)
            };

            Context.ResultRepository.Add(entity);

            await Context.SaveChangesAsync(cancellationToken);

            return ProcessResultModel.Create(entity, serializer);
        }
    }
}
