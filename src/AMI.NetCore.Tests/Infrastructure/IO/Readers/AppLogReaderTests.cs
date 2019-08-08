using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using AMI.Core.IO.Readers;
using NUnit.Framework;

namespace AMI.NetCore.Tests.Infrastructure.IO.Readers
{
    [TestFixture]
    public class AppLogReaderTests : BaseTest
    {
        [Test]
        public void AppLogReader_ReadAsync()
        {
            // Arrange
            OverrideAppOptions(new Dictionary<string, string>()
            {
                { "WorkingDirectory", GetTestPath() }
            });
            var reader = GetService<IAppLogReader>();
            var ct = new CancellationToken();

            // Act
            var list = reader.ReadAsync(ct).Result;
            var first = list.FirstOrDefault();
            var last = list.LastOrDefault();

            // Assert
            Assert.AreEqual(62, list.Count);
            Assert.AreEqual(DateTime.Parse("2019-08-06T19:29:15.2114412Z").ToUniversalTime(), first.Timestamp);
            Assert.AreEqual("Entity Framework Core \"2.2.6-servicing-10079\" initialized '\"SqliteDbContext\"' using provider '\"Microsoft.EntityFrameworkCore.Sqlite\"' with options: \"None\"", first.Message);
            Assert.IsTrue(string.IsNullOrWhiteSpace(first.Level));
            Assert.IsTrue(string.IsNullOrWhiteSpace(first.Exception));
            Assert.AreEqual("9958f5bb", first.EventId);
            Assert.AreEqual("Microsoft.EntityFrameworkCore.Infrastructure", first.SourceContext);
            Assert.AreEqual(DateTime.Parse("2019-08-06T19:30:09.3946479Z").ToUniversalTime(), last.Timestamp);
            Assert.AreEqual("CleanupHostedService stop call ended.", last.Message);
            Assert.IsTrue(string.IsNullOrWhiteSpace(last.Level));
            Assert.IsTrue(string.IsNullOrWhiteSpace(last.Exception));
            Assert.AreEqual("43f50dbc", last.EventId);
            Assert.AreEqual("AMI.Infrastructure.Services.CleanupHostedService", last.SourceContext);
        }
    }
}
