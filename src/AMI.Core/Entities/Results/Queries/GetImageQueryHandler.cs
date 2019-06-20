using System;
using System.IO.Abstractions;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Configurations;
using AMI.Core.Entities.Models;
using AMI.Core.Repositories;
using AMI.Core.Strategies;
using AMI.Domain.Entities;
using AMI.Domain.Exceptions;
using MediatR;

namespace AMI.Core.Entities.Results.Queries
{
    /// <summary>
    /// A handler for the query to get the image file result.
    /// </summary>
    public class GetImageQueryHandler : IRequestHandler<GetImageQuery, FileResultModel>
    {
        private readonly IAmiUnitOfWork context;
        private readonly IAmiConfigurationManager configuration;
        private readonly IFileSystem fileSystem;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetImageQueryHandler"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="fileSystemStrategy">The file system strategy.</param>
        public GetImageQueryHandler(
            IAmiUnitOfWork context,
            IAmiConfigurationManager configuration,
            IFileSystemStrategy fileSystemStrategy)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

            if (fileSystemStrategy == null)
            {
                throw new ArgumentNullException(nameof(fileSystemStrategy));
            }

            fileSystem = fileSystemStrategy.Create(configuration.WorkingDirectory);
        }

        /// <summary>
        /// Handles the specified request.
        /// </summary>
        /// <param name="request">The query request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result contains the image file information.</returns>
        public async Task<FileResultModel> Handle(GetImageQuery request, CancellationToken cancellationToken)
        {
            var entity = await context.ResultRepository
                .GetFirstOrDefaultAsync(e => e.Id == Guid.Parse(request.Id), cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(ResultEntity), request.Id);
            }

            var filename = fileSystem.Path.GetFileName(request.Filename);
            var path = fileSystem.Path.Combine(configuration.WorkingDirectory, entity.BasePath, filename);

            if (!fileSystem.File.Exists(path))
            {
                throw new FileNotFoundException(path);
            }

            var result = new FileResultModel()
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
