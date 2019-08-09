using System;
using System.IO.Abstractions;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Configurations;
using AMI.Core.Entities.Models;
using AMI.Core.Entities.Shared.Commands;
using AMI.Core.Modules;
using AMI.Core.Strategies;
using AMI.Domain.Enums;
using AMI.Domain.Exceptions;

namespace AMI.Core.Entities.Objects.Commands.Delete
{
    /// <summary>
    /// A handler for delete command requests.
    /// </summary>
    public class DeleteCommandHandler : BaseCommandRequestHandler<DeleteObjectCommand, bool>
    {
        private readonly IAppConfiguration configuration;
        private readonly IFileSystem fileSystem;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteCommandHandler"/> class.
        /// </summary>
        /// <param name="module">The command handler module.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="fileSystemStrategy">The file system strategy.</param>
        public DeleteCommandHandler(
            ICommandHandlerModule module,
            IAppConfiguration configuration,
            IFileSystemStrategy fileSystemStrategy)
            : base(module)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

            if (fileSystemStrategy == null)
            {
                throw new ArgumentNullException(nameof(fileSystemStrategy));
            }

            fileSystem = fileSystemStrategy.Create(configuration.Options.WorkingDirectory);
        }

        /// <inheritdoc/>
        protected override async Task<bool> ProtectedHandleAsync(DeleteObjectCommand request, CancellationToken cancellationToken)
        {
            Context.BeginTransaction();

            var entity = await Context.ObjectRepository
                .GetFirstOrDefaultAsync(e => e.Id == Guid.Parse(request.Id), cancellationToken);

            if (entity == null)
            {
                return true;
            }

            if (!IdentityService.IsAuthorized(entity.UserId))
            {
                throw new ForbiddenException("Not authorized");
            }

            Context.ObjectRepository.Remove(entity);

            var directoryName = fileSystem.Path.GetDirectoryName(entity.SourcePath);
            if (!directoryName.EndsWith(entity.Id.ToString()))
            {
                throw new AmiException(string.Format(
                    "The directory name of object {0} ends with an unexpected name: {1}",
                    entity.Id,
                    directoryName));
            }

            await Context.CommitTransactionAsync(cancellationToken);

            fileSystem.Directory.Delete(fileSystem.Path.Combine(configuration.Options.WorkingDirectory, directoryName), true);

            var result = ObjectModel.Create(entity);

            await Gateway.NotifyGroupsAsync(
                entity.UserId,
                GatewayOpCode.Dispatch,
                GatewayEvent.DeleteObject,
                result,
                cancellationToken);

            return true;
        }
    }
}
