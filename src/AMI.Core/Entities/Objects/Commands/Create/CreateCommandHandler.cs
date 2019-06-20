using System;
using System.IO.Abstractions;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Configurations;
using AMI.Core.Constants;
using AMI.Core.Entities.Models;
using AMI.Core.Entities.Shared.Commands;
using AMI.Core.Repositories;
using AMI.Core.Services;
using AMI.Core.Strategies;
using AMI.Domain.Entities;

namespace AMI.Core.Entities.Objects.Commands.Create
{
    /// <summary>
    /// A handler for create command requests.
    /// </summary>
    /// <seealso cref="BaseCommandRequestHandler{CreateObjectCommand, ObjectModel}" />
    public class CreateCommandHandler : BaseCommandRequestHandler<CreateObjectCommand, ObjectModel>
    {
        private readonly IAmiUnitOfWork context;
        private readonly IIdGenService idGenService;
        private readonly IApplicationConstants constants;
        private readonly IAmiConfigurationManager configuration;
        private readonly IFileSystem fileSystem;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateCommandHandler"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="idGenService">The service to generate unique identifiers.</param>
        /// <param name="constants">The application constants.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="fileSystemStrategy">The file system strategy.</param>
        public CreateCommandHandler(
            IAmiUnitOfWork context,
            IIdGenService idGenService,
            IApplicationConstants constants,
            IAmiConfigurationManager configuration,
            IFileSystemStrategy fileSystemStrategy)
            : base()
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.idGenService = idGenService ?? throw new ArgumentNullException(nameof(idGenService));
            this.constants = constants ?? throw new ArgumentNullException(nameof(constants));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

            if (fileSystemStrategy == null)
            {
                throw new ArgumentNullException(nameof(fileSystemStrategy));
            }

            fileSystem = fileSystemStrategy.Create(configuration.WorkingDirectory);
        }

        /// <inheritdoc/>
        protected override async Task<ObjectModel> ProtectedHandleAsync(CreateObjectCommand request, CancellationToken cancellationToken)
        {
            context.BeginTransaction();

            Guid guid = idGenService.CreateId();
            string path = fileSystem.Path.Combine("Binary", "Objects", guid.ToString());
            string destFilename = string.Concat(guid.ToString(), fileSystem.Path.GetExtension(request.OriginalFilename));
            string destPath = fileSystem.Path.Combine(path, destFilename);

            fileSystem.Directory.CreateDirectory(fileSystem.Path.Combine(configuration.WorkingDirectory, path));
            fileSystem.File.Move(request.SourcePath, fileSystem.Path.Combine(configuration.WorkingDirectory, destPath));

            var entity = new ObjectEntity()
            {
                Id = guid,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow,
                OriginalFilename = request.OriginalFilename,
                SourcePath = destPath
            };

            context.ObjectRepository.Add(entity);

            await context.SaveChangesAsync(cancellationToken);

            context.CommitTransaction();

            return ObjectModel.Create(entity);
        }
    }
}
