using System;
using System.IO.Abstractions;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Configurations;
using AMI.Core.Constants;
using AMI.Core.Entities.Models;
using AMI.Core.Entities.Shared.Commands;
using AMI.Core.IO.Generators;
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
        private readonly IIdGenerator idGenerator;
        private readonly IApplicationConstants constants;
        private readonly IAppConfiguration configuration;
        private readonly IFileSystem fileSystem;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateCommandHandler"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="gateway">The gateway service.</param>
        /// <param name="idGenerator">The generator for unique identifiers.</param>
        /// <param name="constants">The application constants.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="fileSystemStrategy">The file system strategy.</param>
        public CreateCommandHandler(
            IAmiUnitOfWork context,
            IGatewayService gateway,
            IIdGenerator idGenerator,
            IApplicationConstants constants,
            IAppConfiguration configuration,
            IFileSystemStrategy fileSystemStrategy)
            : base(context, gateway)
        {
            this.idGenerator = idGenerator ?? throw new ArgumentNullException(nameof(idGenerator));
            this.constants = constants ?? throw new ArgumentNullException(nameof(constants));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

            if (fileSystemStrategy == null)
            {
                throw new ArgumentNullException(nameof(fileSystemStrategy));
            }

            fileSystem = fileSystemStrategy.Create(configuration.Options.WorkingDirectory);
        }

        /// <inheritdoc/>
        protected override async Task<ObjectModel> ProtectedHandleAsync(CreateObjectCommand request, CancellationToken cancellationToken)
        {
            Context.BeginTransaction();

            // TODO: support custom extensions
            string fileExtension = fileSystem.Path.GetExtension(request.OriginalFilename);

            Guid guid = idGenerator.GenerateId();
            string path = fileSystem.Path.Combine("Binary", "Objects", guid.ToString());
            string destFilename = string.Concat(guid.ToString(), fileExtension);
            string destPath = fileSystem.Path.Combine(path, destFilename);

            fileSystem.Directory.CreateDirectory(fileSystem.Path.Combine(configuration.Options.WorkingDirectory, path));
            fileSystem.File.Move(request.SourcePath, fileSystem.Path.Combine(configuration.Options.WorkingDirectory, destPath));

            var entity = new ObjectEntity()
            {
                Id = guid,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow,
                OriginalFilename = request.OriginalFilename,
                SourcePath = destPath
            };

            Context.ObjectRepository.Add(entity);

            await Context.SaveChangesAsync(cancellationToken);

            Context.CommitTransaction();

            return ObjectModel.Create(entity);
        }
    }
}
