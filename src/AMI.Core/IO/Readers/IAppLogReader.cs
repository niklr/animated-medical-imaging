using System.Collections.Generic;
using AMI.Domain.Entities;

namespace AMI.Core.IO.Readers
{
    public interface IAppLogReader
    {
        IList<AppLogEntity> Read();
    }
}
