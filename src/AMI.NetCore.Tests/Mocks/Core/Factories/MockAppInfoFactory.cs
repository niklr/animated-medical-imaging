using System.Reflection;
using AMI.Core.Entities.Models;
using AMI.Core.Extensions.StringExtensions;
using AMI.Core.Factories;

namespace AMI.NetCore.Tests.Mocks.Core.Factories
{
    internal class MockAppInfoFactory : IAppInfoFactory
    {
        public AppInfo Create()
        {
            var assembly = Assembly.GetExecutingAssembly();
            return new AppInfo(assembly.GetName().Name, assembly.GetName().Version.ToStringInvariant());
        }
    }
}
