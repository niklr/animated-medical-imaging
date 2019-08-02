using System;
using System.IO.Abstractions;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Configurations;
using AMI.Core.Entities.Models;
using AMI.Core.Entities.Shared.Queries;
using AMI.Core.Modules;
using AMI.Core.Strategies;
using AMI.Domain.Entities;
using AMI.Domain.Exceptions;

namespace AMI.Core.Entities.Results.Queries.GetImage
{
    /// <summary>
    /// A query handler to get the image file result.
    /// </summary>
    public class GetImageQueryHandler : BaseQueryRequestHandler<GetImageQuery, FileByteResultModel>
    {
        private readonly IAppConfiguration configuration;
        private readonly IFileSystem fileSystem;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetImageQueryHandler"/> class.
        /// </summary>
        /// <param name="module">The query handler module.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="fileSystemStrategy">The file system strategy.</param>
        public GetImageQueryHandler(
            IQueryHandlerModule module,
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
        protected override async Task<FileByteResultModel> ProtectedHandleAsync(GetImageQuery request, CancellationToken cancellationToken)
        {
            var entity = await Context.ResultRepository
                .GetFirstOrDefaultAsync(e => e.Id == Guid.Parse(request.Id), cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(ResultEntity), request.Id);
            }

            var filename = fileSystem.Path.GetFileName(request.Filename);
            var path = fileSystem.Path.Combine(configuration.Options.WorkingDirectory, entity.BasePath, filename);

            if (!fileSystem.File.Exists(path))
            {
                throw new FileNotFoundException(path);
            }

            var result = new FileByteResultModel()
            {
                FileContents = fileSystem.File.ReadAllBytes(path)
            };

            string fileExtension = fileSystem.Path.GetExtension(filename);
            switch (fileExtension)
            {
                case ".png":
                    result.ContentType = "image/png";
                    break;
                case ".gif":
                    result.ContentType = "image/gif";
                    break;
                default:
                    throw new AmiException($"Image file extension ({fileExtension}) is not supported.");
            }

            return result;
        }
    }
}
