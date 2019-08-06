using System;
using System.Collections.Generic;
using System.IO;
using AMI.Core.Constants;
using AMI.Core.Entities.Models;
using AMI.Core.IO.Readers;
using AMI.Core.IO.Serializers;
using AMI.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace AMI.Infrastructure.IO.Readers
{
    public class AppLogReader : IAppLogReader
    {
        private readonly ILogger logger;
        private readonly IAppOptions options;
        private readonly IApplicationConstants constants;
        private readonly IDefaultJsonSerializer serializer;

        public AppLogReader(ILoggerFactory loggerFactory, IAppOptions options, IApplicationConstants constants, IDefaultJsonSerializer serializer)
        {
            logger = loggerFactory?.CreateLogger<AppLogReader>() ?? throw new ArgumentNullException(nameof(loggerFactory));
            this.options = options ?? throw new ArgumentNullException(nameof(options));
            this.constants = constants ?? throw new ArgumentNullException(nameof(constants));
            this.serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
        }

        /// <inheritdoc/>
        public IList<AppLogEntity> Read()
        {
            List<AppLogEntity> items = new List<AppLogEntity>();

            try
            {
                using (StreamReader reader = new StreamReader(Path.Combine(options.WorkingDirectory, "Logs", constants.LogFilename)))
                {
                    string json = reader.ReadToEnd();
                    items = serializer.Deserialize<List<AppLogEntity>>(json);
                }
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
            }

            return new List<AppLogEntity>();
        }
    }
}
