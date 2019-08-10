using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Configurations;
using AMI.Core.Constants;
using AMI.Core.IO.Readers;
using AMI.Core.IO.Serializers;
using AMI.Core.Strategies;
using AMI.Domain.Entities;
using AMI.Domain.Exceptions;
using RNS.Framework.Extensions.MutexExtensions;
using RNS.Framework.Extensions.Reflection;

namespace AMI.Infrastructure.IO.Readers
{
    /// <summary>
    /// A reader for application logs.
    /// </summary>
    public class AppLogReader : IAppLogReader
    {
        private static Mutex processMutex;

        private readonly IAppConfiguration configuration;
        private readonly IApplicationConstants constants;
        private readonly IFileSystemStrategy fileSystemStrategy;
        private readonly IDefaultJsonSerializer serializer;

        private string logFilePath;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppLogReader"/> class.
        /// </summary>
        /// <param name="configuration">The application configuration.</param>
        /// <param name="constants">The application constants.</param>
        /// <param name="fileSystemStrategy">The file system strategy.</param>
        /// <param name="serializer">The JSON serializer.</param>
        public AppLogReader(
            IAppConfiguration configuration,
            IApplicationConstants constants,
            IFileSystemStrategy fileSystemStrategy,
            IDefaultJsonSerializer serializer)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.constants = constants ?? throw new ArgumentNullException(nameof(constants));
            this.fileSystemStrategy = fileSystemStrategy ?? throw new ArgumentNullException(nameof(fileSystemStrategy));
            this.serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
        }

        /// <inheritdoc/>
        public Task<IList<AppLogEntity>> ReadAsync(CancellationToken ct)
        {
            processMutex = new Mutex(false, this.GetMethodName());

            return processMutex.Execute(new TimeSpan(0, 0, 2), () =>
            {
                ct.ThrowIfCancellationRequested();

                if (logFilePath == null)
                {
                    InitLogFilePath();
                }

                IList<AppLogEntity> list = new List<AppLogEntity>();

                var fs = fileSystemStrategy.Create(configuration.Options.WorkingDirectory);
                if (fs == null)
                {
                    throw new UnexpectedNullException("Filesystem could not be created based on the working directory.");
                }

                using (var stream = fs.FileStream.Create(logFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, 0x1000, FileOptions.SequentialScan))
                using (var reader = new StreamReader(stream, Encoding.UTF8))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        ct.ThrowIfCancellationRequested();

                        if (!string.IsNullOrWhiteSpace(line))
                        {
                            try
                            {
                                var serilog = serializer.Deserialize<CompactSerilog>(line);
                                list.Add(serilog.ToAppLogEntity());
                            }
                            catch (Exception e)
                            {
                                Debug.WriteLine(e.Message);
                            }
                        }
                    }
                }

                return Task.FromResult(list);
            });
        }

        private void InitLogFilePath()
        {
            var fs = fileSystemStrategy.Create(configuration.Options.WorkingDirectory);
            string path = fs.Path.Combine(configuration.Options.WorkingDirectory, constants.LogFilePath);

            if (!Uri.TryCreate(path, UriKind.RelativeOrAbsolute, out var logFilePath))
            {
                throw new Exception($"Invalid log file path '{constants.LogFilePath}'.");
            }

            this.logFilePath = fs.Path.GetFullPath(path);
        }

        /// <summary>
        /// Based on https://github.com/serilog/serilog-formatting-compact
        /// </summary>
        [DataContract]
        private class CompactSerilog
        {
            [DataMember(Name = "@t")]
            public DateTime Timestamp { get; set; }

            [DataMember(Name = "@m")]
            public string Message { get; set; }

            [DataMember(Name = "@l")]
            public string Level { get; set; }

            [DataMember(Name = "@x")]
            public string Exception { get; set; }

            [DataMember(Name = "@i")]
            public string EventId { get; set; }

            [DataMember(Name = "SourceContext")]
            public string SourceContext { get; set; }

            public AppLogEntity ToAppLogEntity()
            {
                return new AppLogEntity()
                {
                    Timestamp = Timestamp,
                    Message = Message,
                    Level = Level,
                    Exception = Exception,
                    EventId = EventId,
                    SourceContext = SourceContext
                };
            }
        }
    }
}
