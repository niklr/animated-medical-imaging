using System;
using System.IO.Abstractions;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Configurations;
using AMI.Core.Entities.Models;
using AMI.Core.Entities.Shared.Commands;
using AMI.Core.Repositories;
using AMI.Core.Strategies;
using AMI.Domain.Entities;
using AMI.Domain.Exceptions;

namespace AMI.Core.Entities.Objects.Commands.Delete
{
    /// <summary>
    /// A handler for delete command requests.
    /// </summary>
    /// <seealso cref="BaseCommandRequestHandler{DeleteObjectCommand, ObjectModel}" />
    public class DeleteCommandHandler : BaseCommandRequestHandler<DeleteObjectCommand, ObjectModel>
    {
        private readonly IAmiUnitOfWork context;
        private readonly IAppConfiguration configuration;
        private readonly IFileSystem fileSystem;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteCommandHandler"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="fileSystemStrategy">The file system strategy.</param>
        public DeleteCommandHandler(
            IAmiUnitOfWork context,
            IAppConfiguration configuration,
            IFileSystemStrategy fileSystemStrategy)
            : base()
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

            if (fileSystemStrategy == null)
            {
                throw new ArgumentNullException(nameof(fileSystemStrategy));
            }

            fileSystem = fileSystemStrategy.Create(configuration.Options.WorkingDirectory);
        }

        /// <inheritdoc/>
        protected override async Task<ObjectModel> ProtectedHandleAsync(DeleteObjectCommand request, CancellationToken cancellationToken)
        {
            context.BeginTransaction();

            var entity = await context.ObjectRepository
                .GetFirstOrDefaultAsync(e => e.Id == Guid.Parse(request.Id), cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(ObjectEntity), request.Id);
            }

            context.ObjectRepository.Remove(entity);

            await context.SaveChangesAsync(cancellationToken);

            var directoryName = fileSystem.Path.GetDirectoryName(entity.SourcePath);
            if (!directoryName.EndsWith(entity.Id.ToString()))
            {
                throw new AmiException(string.Format(
                    "The directory name of object {0} ends with an unexpected name: {1}",
                    entity.Id,
                    directoryName));
            }

            context.CommitTransaction();

            fileSystem.Directory.Delete(fileSystem.Path.Combine(configuration.Options.WorkingDirectory, directoryName), true);

            return ObjectModel.Create(entity);
        }
    }
}
