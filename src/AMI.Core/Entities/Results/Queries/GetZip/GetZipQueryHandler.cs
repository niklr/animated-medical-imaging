using System;
using System.IO;
using System.IO.Abstractions;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Configurations;
using AMI.Core.Entities.Models;
using AMI.Core.Entities.Shared.Queries;
using AMI.Core.IO.Writers;
using AMI.Core.Modules;
using AMI.Core.Strategies;
using AMI.Domain.Entities;
using AMI.Domain.Enums;
using AMI.Domain.Exceptions;

namespace AMI.Core.Entities.Results.Queries.GetZip
{
    /// <summary>
    /// A query handler to get the result as a zip.
    /// </summary>
    public class GetZipQueryHandler : BaseQueryRequestHandler<GetZipQuery, FileByteResultModel>
    {
        private readonly IAppConfiguration configuration;
        private readonly IArchiveWriter writer;
        private readonly IFileSystem fileSystem;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetZipQueryHandler"/> class.
        /// </summary>
        /// <param name="module">The query handler module.</param>
        /// <param name="configuration">The application configuration.</param>
        /// <param name="writer">The archive writer.</param>
        /// <param name="fileSystemStrategy">The file system strategy.</param>
        public GetZipQueryHandler(
            IQueryHandlerModule module,
            IAppConfiguration configuration,
            IArchiveWriter writer,
            IFileSystemStrategy fileSystemStrategy)
            : base(module)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.writer = writer ?? throw new ArgumentNullException(nameof(writer));

            if (fileSystemStrategy == null)
            {
                throw new ArgumentNullException(nameof(fileSystemStrategy));
            }

            fileSystem = fileSystemStrategy.Create(configuration.Options.WorkingDirectory);
        }

        /// <inheritdoc/>
        protected override async Task<FileByteResultModel> ProtectedHandleAsync(GetZipQuery request, CancellationToken cancellationToken)
        {
            var entity = await Context.ResultRepository
                .GetFirstOrDefaultAsync(e => e.Id == Guid.Parse(request.Id), cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(ResultEntity), request.Id);
            }

            if (string.IsNullOrWhiteSpace(entity.BasePath))
            {
                throw new UnexpectedNullException($"The base path of result {request.Id} is null.");
            }

            var fullBasePath = fileSystem.Path.Combine(configuration.Options.WorkingDirectory, entity.BasePath);
            var items = fileSystem.Directory.EnumerateFiles(fullBasePath, "*.*", SearchOption.TopDirectoryOnly);

            var archive = writer.Create(CompressionType.None);
            await writer.AddFilesAsync(items, dfp => dfp, en => en.Substring(fullBasePath.Length), archive, cancellationToken);
            var stream = new MemoryStream();
            writer.Write(stream, archive);

            var result = new FileByteResultModel()
            {
                FileContents = stream.ToArray(),
                ContentType = "application/zip",
                FileDownloadName = $"{Constants.ApplicationNameShort}_{request.Id}_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.zip"
            };

            return result;
        }
    }
}
