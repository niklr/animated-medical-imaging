using System;
using System.IO.Abstractions;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Configurations;
using AMI.Core.Constants;
using AMI.Core.Entities.Models;
using AMI.Core.Entities.Shared.Commands;
using AMI.Core.IO.Generators;
using AMI.Core.Modules;
using AMI.Core.Strategies;
using AMI.Domain.Entities;
using AMI.Domain.Enums;
using AMI.Domain.Enums.Auditing;
using AMI.Domain.Exceptions;
using RNS.Framework.Extensions.MutexExtensions;
using RNS.Framework.Extensions.Reflection;

namespace AMI.Core.Entities.Objects.Commands.Create
{
    /// <summary>
    /// A handler for create command requests.
    /// </summary>
    public class CreateCommandHandler : BaseCommandRequestHandler<CreateObjectCommand, ObjectModel>
    {
        private static Mutex processMutex;

        private readonly IIdGenerator idGenerator;
        private readonly IApplicationConstants constants;
        private readonly IApiConfiguration apiConfiguration;
        private readonly IAppConfiguration appConfiguration;
        private readonly IFileSystem fileSystem;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateCommandHandler"/> class.
        /// </summary>
        /// <param name="module">The command handler module.</param>
        /// <param name="idGenerator">The generator for unique identifiers.</param>
        /// <param name="constants">The application constants.</param>
        /// <param name="apiConfiguration">The API configuration.</param>
        /// <param name="appConfiguration">The application configuration.</param>
        /// <param name="fileSystemStrategy">The file system strategy.</param>
        public CreateCommandHandler(
            ICommandHandlerModule module,
            IIdGenerator idGenerator,
            IApplicationConstants constants,
            IApiConfiguration apiConfiguration,
            IAppConfiguration appConfiguration,
            IFileSystemStrategy fileSystemStrategy)
            : base(module)
        {
            this.idGenerator = idGenerator ?? throw new ArgumentNullException(nameof(idGenerator));
            this.constants = constants ?? throw new ArgumentNullException(nameof(constants));
            this.apiConfiguration = apiConfiguration ?? throw new ArgumentNullException(nameof(apiConfiguration));
            this.appConfiguration = appConfiguration ?? throw new ArgumentNullException(nameof(appConfiguration));

            if (fileSystemStrategy == null)
            {
                throw new ArgumentNullException(nameof(fileSystemStrategy));
            }

            fileSystem = fileSystemStrategy.Create(appConfiguration.Options.WorkingDirectory);
        }

        /// <inheritdoc/>
        protected override SubEventType SubEventType
        {
            get
            {
                return SubEventType.CreateObject;
            }
        }

        /// <inheritdoc/>
        protected override async Task<ObjectModel> ProtectedHandleAsync(CreateObjectCommand request, CancellationToken cancellationToken)
        {
            var principal = PrincipalProvider.GetPrincipal();
            if (principal == null)
            {
                throw new ForbiddenException("Not authenticated");
            }

            int objectLimit = 0;
            if (principal.IsInRole(RoleType.User))
            {
                objectLimit = apiConfiguration.Options.ObjectLimit;
            }
            else
            {
                objectLimit = apiConfiguration.Options.ObjectLimitAnonymous;
            }

            processMutex = new Mutex(false, this.GetMethodName());

            return await processMutex.Execute(new TimeSpan(0, 0, 2), async () =>
            {
                if (objectLimit > 0)
                {
                    var count = await Context.ObjectRepository.CountAsync(e => e.UserId == principal.Identity.Name, cancellationToken);
                    if (count >= objectLimit)
                    {
                        throw new AmiException($"The object limit of {objectLimit} has been reached.");
                    }
                }

                string fileExtension = fileSystem.Path.GetExtension(request.OriginalFilename);

                Guid guid = idGenerator.GenerateId();
                string path = fileSystem.Path.Combine("Binary", "Objects", guid.ToString());
                string destFilename = string.Concat(guid.ToString(), fileExtension);
                string destPath = fileSystem.Path.Combine(path, destFilename);

                fileSystem.Directory.CreateDirectory(fileSystem.Path.Combine(appConfiguration.Options.WorkingDirectory, path));
                fileSystem.File.Move(request.SourcePath, fileSystem.Path.Combine(appConfiguration.Options.WorkingDirectory, destPath));

                var entity = new ObjectEntity()
                {
                    Id = guid,
                    CreatedDate = DateTime.UtcNow,
                    ModifiedDate = DateTime.UtcNow,
                    OriginalFilename = request.OriginalFilename,
                    SourcePath = destPath,
                    UserId = principal.Identity.Name
                };

                Context.ObjectRepository.Add(entity);

                await Context.SaveChangesAsync(cancellationToken);

                var result = ObjectModel.Create(entity);

                await Gateway.NotifyGroupsAsync(
                    entity.UserId,
                    GatewayOpCode.Dispatch,
                    GatewayEvent.CreateObject,
                    result,
                    cancellationToken);

                return result;
            });
        }
    }
}
