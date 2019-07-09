using System;
using System.IO.Abstractions;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Configurations;
using AMI.Core.Entities.Models;
using AMI.Core.Entities.Results.Commands.ProcessPath;
using AMI.Core.Entities.Shared.Commands;
using AMI.Core.IO.Serializers;
using AMI.Core.Repositories;
using AMI.Core.Services;
using AMI.Core.Strategies;
using AMI.Domain.Entities;
using AMI.Domain.Exceptions;
using MediatR;

namespace AMI.Core.Entities.Results.Commands.ProcessObject
{
    /// <summary>
    /// A handler for process command requests.
    /// </summary>
    public class ProcessCommandHandler : BaseCommandRequestHandler<ProcessObjectCommand, ProcessResultModel>
    {
        private readonly IIdGenService idGenService;
        private readonly IDefaultJsonSerializer serializer;
        private readonly IMediator mediator;
        private readonly IAppConfiguration configuration;
        private readonly IFileSystem fileSystem;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessCommandHandler"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="gateway">The gateway service.</param>
        /// <param name="idGenService">The service to generate unique identifiers.</param>
        /// <param name="serializer">The JSON serializer.</param>
        /// <param name="mediator">The mediator.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="fileSystemStrategy">The file system strategy.</param>
        public ProcessCommandHandler(
            IAmiUnitOfWork context,
            IGatewayService gateway,
            IIdGenService idGenService,
            IDefaultJsonSerializer serializer,
            IMediator mediator,
            IAppConfiguration configuration,
            IFileSystemStrategy fileSystemStrategy)
            : base(context, gateway)
        {
            this.idGenService = idGenService ?? throw new ArgumentNullException(nameof(idGenService));
            this.serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

            if (fileSystemStrategy == null)
            {
                throw new ArgumentNullException(nameof(fileSystemStrategy));
            }

            fileSystem = fileSystemStrategy.Create(configuration.Options.WorkingDirectory);
        }

        /// <inheritdoc/>
        protected override async Task<ProcessResultModel> ProtectedHandleAsync(ProcessObjectCommand request, CancellationToken cancellationToken)
        {
            Context.BeginTransaction();

            var objectEntity = await Context.ObjectRepository.GetFirstOrDefaultAsync(e => e.Id == Guid.Parse(request.Id), cancellationToken);
            if (objectEntity == null)
            {
                throw new UnexpectedNullException($"{nameof(ObjectEntity)} not found.");
            }

            // Create temporary directory and use it as destination path
            var tempDestPath = fileSystem.Path.Combine(configuration.Options.WorkingDirectory, "Temp", Guid.NewGuid().ToString());
            fileSystem.Directory.CreateDirectory(tempDestPath);

            // TODO: extract if needed, update the ObjectEntity.UncompressedPath
            // and use the extracted directory as source path
            var pathRequest = new ProcessPathCommand()
            {
                SourcePath = fileSystem.Path.Combine(configuration.Options.WorkingDirectory, objectEntity.SourcePath),
                DestinationPath = tempDestPath,
                DesiredSize = request.DesiredSize,
                AmountPerAxis = request.AmountPerAxis,
                AxisTypes = request.AxisTypes,
                ImageFormat = request.ImageFormat,
                BezierEasingTypePerAxis = request.BezierEasingTypePerAxis,
                BezierEasingTypeCombined = request.BezierEasingTypeCombined,
                Grayscale = request.Grayscale
            };

            // Start the processing
            var result = await mediator.Send(pathRequest, cancellationToken);
            if (result == null)
            {
                throw new UnexpectedNullException($"The processing result of object {request.Id} was null.");
            }

            // Ensure "Results" directory exists
            var resultsDirectoryName = "Results";
            var resultsPath = fileSystem.Path.Combine(configuration.Options.WorkingDirectory, resultsDirectoryName);
            fileSystem.Directory.CreateDirectory(resultsPath);

            // Move content of temporary directory and delete it
            var baseDestPath = fileSystem.Path.Combine(resultsDirectoryName, result.Id);
            var destPath = fileSystem.Path.Combine(configuration.Options.WorkingDirectory, baseDestPath);
            fileSystem.Directory.Move(tempDestPath, destPath);

            var resultEntity = await Context.ResultRepository.GetFirstOrDefaultAsync(e => e.Id == Guid.Parse(result.Id), cancellationToken);
            if (resultEntity == null)
            {
                throw new UnexpectedNullException($"{nameof(ResultEntity)} not found.");
            }

            // Update BaseFsPath of ResultEntity
            resultEntity.BasePath = baseDestPath;
            resultEntity.ModifiedDate = DateTime.UtcNow;
            Context.ResultRepository.Update(resultEntity);

            await Context.SaveChangesAsync(cancellationToken);

            Context.CommitTransaction();

            return result;
        }
    }
}
