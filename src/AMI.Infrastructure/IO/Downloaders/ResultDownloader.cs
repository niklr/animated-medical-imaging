﻿using System;
using System.IO;
using System.IO.Abstractions;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Configurations;
using AMI.Core.Entities.Results.Queries.GetById;
using AMI.Core.IO.Downloaders;
using AMI.Core.IO.Writers;
using AMI.Core.Repositories;
using AMI.Core.Strategies;
using AMI.Domain.Entities;
using AMI.Domain.Enums;
using AMI.Domain.Exceptions;
using MediatR;

namespace AMI.Infrastructure.IO.Downloaders
{
    /// <summary>
    /// A downloader for results.
    /// </summary>
    /// <seealso cref="IResultDownloader" />
    public class ResultDownloader : IResultDownloader
    {
        private readonly IMediator mediator;
        private readonly IAmiUnitOfWork context;
        private readonly IAmiConfigurationManager configuration;
        private readonly ICompressibleWriter writer;
        private readonly IFileSystem fileSystem;

        public ResultDownloader(
            IMediator mediator,
            IAmiUnitOfWork context,
            IAmiConfigurationManager configuration,
            ICompressibleWriter writer,
            IFileSystemStrategy fileSystemStrategy)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.writer = writer ?? throw new ArgumentNullException(nameof(writer));

            if (fileSystemStrategy == null)
            {
                throw new ArgumentNullException(nameof(fileSystemStrategy));
            }

            fileSystem = fileSystemStrategy.Create(configuration.WorkingDirectory);
        }

        public async Task SaveAsync(string id, Stream stream, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            if (ct == null)
            {
                throw new ArgumentNullException(nameof(ct));
            }

            var entity = await context.ResultRepository.GetFirstOrDefaultAsync(e => e.Id == Guid.Parse(id), ct);
            if (entity == null)
            {
                throw new NotFoundException(nameof(ResultEntity), id);
            }

            if (string.IsNullOrWhiteSpace(entity.BasePath))
            {
                throw new UnexpectedNullException($"The base path of result {id} is null.");
            }

            var fullBasePath = fileSystem.Path.Combine(configuration.WorkingDirectory, entity.BasePath);
            var items = fileSystem.Directory.EnumerateFiles(fullBasePath, "*.*", SearchOption.TopDirectoryOnly);

            var archive = writer.Create(CompressionType.None);
            await writer.AddFilesAsync(items, dfp => dfp, en => en.Substring(fullBasePath.Length), archive, ct);
            writer.Write(stream, archive);
        }
    }
}
