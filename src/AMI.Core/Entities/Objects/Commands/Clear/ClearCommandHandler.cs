using System;
using System.IO.Abstractions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Configurations;
using AMI.Core.Entities.Objects.Commands.Delete;
using AMI.Core.Entities.Shared.Commands;
using AMI.Core.Modules;
using AMI.Core.Strategies;
using MediatR;

namespace AMI.Core.Entities.Objects.Commands.Clear
{
    /// <summary>
    /// A handler for clear command requests.
    /// </summary>
    public class ClearCommandHandler : BaseCommandRequestHandler<ClearObjectsCommand, bool>
    {
        private readonly IMediator mediator;
        private readonly IAppConfiguration configuration;
        private readonly IFileSystem fileSystem;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClearCommandHandler"/> class.
        /// </summary>
        /// <param name="module">The command handler module.</param>
        /// <param name="mediator">The mediator.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="fileSystemStrategy">The file system strategy.</param>
        public ClearCommandHandler(
            ICommandHandlerModule module,
            IMediator mediator,
            IAppConfiguration configuration,
            IFileSystemStrategy fileSystemStrategy)
            : base(module)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

            if (fileSystemStrategy == null)
            {
                throw new ArgumentNullException(nameof(fileSystemStrategy));
            }

            fileSystem = fileSystemStrategy.Create(configuration.Options.WorkingDirectory);
        }

        /// <inheritdoc/>
        protected override async Task<bool> ProtectedHandleAsync(ClearObjectsCommand request, CancellationToken cancellationToken)
        {
            if (request.RefDate.HasValue)
            {
                var refDate = request.RefDate.Value.ToUniversalTime();
                var entities = Context.ObjectRepository.GetQuery().Where(e => e.CreatedDate <= refDate).ToList();
                foreach (var entity in entities)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    var command = new DeleteObjectCommand()
                    {
                        Id = entity.Id.ToString()
                    };

                    await mediator.Send(command, cancellationToken);
                }
            }
            else
            {
                if (fileSystem.Directory.Exists(configuration.Options.WorkingDirectory))
                {
                    var directories = fileSystem.Directory.EnumerateDirectories(configuration.Options.WorkingDirectory);
                    foreach (var directory in directories)
                    {
                        fileSystem.Directory.Delete(directory, true);
                    }
                }
            }

            await Task.CompletedTask;

            return true;
        }
    }
}
