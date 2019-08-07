using System;
using System.Threading;
using AMI.Core.IO.Readers;
using AMI.Core.Repositories;
using AMI.Domain.Entities;
using RNS.Framework.Extensions.MutexExtensions;
using RNS.Framework.Extensions.Reflection;

namespace AMI.Infrastructure.Repositories
{
    /// <summary>
    /// A repository for application logs.
    /// </summary>
    public class AppLogRepository : BaseRepository<AppLogEntity>, IAppLogRepository
    {
        private static Mutex processMutex;
        private readonly IAppLogReader reader;
        private IRepository<AppLogEntity> repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppLogRepository" /> class.
        /// </summary>
        /// <param name="reader">The application log reader.</param>
        public AppLogRepository(IAppLogReader reader)
        {
            this.reader = reader ?? throw new ArgumentNullException(nameof(reader));
        }

        /// <inheritdoc/>
        protected override IRepository<AppLogEntity> Repository
        {
            get
            {
                processMutex = new Mutex(false, this.GetMethodName());

                return processMutex.Execute(new TimeSpan(0, 0, 2), () =>
                {
                    if (repository == null)
                    {
                        repository = new ListRepository<AppLogEntity>(reader.ReadAsync(default).Result);
                    }

                    return repository;
                });
            }
        }
    }
}
