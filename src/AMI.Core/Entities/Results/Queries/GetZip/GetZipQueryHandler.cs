using System;
using System.IO;
using System.IO.Abstractions;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Configurations;
using AMI.Core.Constants;
using AMI.Core.Entities.Models;
using AMI.Core.IO.Writers;
using AMI.Core.Repositories;
using AMI.Core.Strategies;
using AMI.Domain.Entities;
using AMI.Domain.Enums;
using AMI.Domain.Exceptions;
using MediatR;

namespace AMI.Core.Entities.Results.Queries.GetZip
{
    /// <summary>
    /// A handler for the query to get the result as a zip.
    /// </summary>
    public class GetZipQueryHandler : IRequestHandler<GetZipQuery, FileByteResultModel>
    {
        private readonly IAmiUnitOfWork context;
        private readonly IApplicationConstants constants;
        private readonly IAppConfiguration configuration;
        private readonly ICompressibleWriter writer;
        private readonly IFileSystem fileSystem;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetZipQueryHandler"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="constants">The application constants.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="writer">The compressible writer.</param>
        /// <param name="fileSystemStrategy">The file system strategy.</param>
        public GetZipQueryHandler(
            IAmiUnitOfWork context,
            IApplicationConstants constants,
            IAppConfiguration configuration,
            ICompressibleWriter writer,
            IFileSystemStrategy fileSystemStrategy)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.constants = constants ?? throw new ArgumentNullException(nameof(constants));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.writer = writer ?? throw new ArgumentNullException(nameof(writer));

            if (fileSystemStrategy == null)
            {
                throw new ArgumentNullException(nameof(fileSystemStrategy));
            }

            fileSystem = fileSystemStrategy.Create(configuration.Options.WorkingDirectory);
        }

        /// <summary>
        /// Handles the specified request.
        /// </summary>
        /// <param name="request">The query request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result contains the image file information.</returns>
        public async Task<FileByteResultModel> Handle(GetZipQuery request, CancellationToken cancellationToken)
        {
            var entity = await context.ResultRepository
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
                FileDownloadName = $"{constants.ApplicationNameShort}_{request.Id}_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.zip"
            };

            return result;
        }
    }
}
