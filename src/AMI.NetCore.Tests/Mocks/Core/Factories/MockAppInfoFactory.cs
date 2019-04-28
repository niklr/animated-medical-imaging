using System.Reflection;
using AMI.Core.Extensions.StringExtensions;
using AMI.Core.Factories;
using AMI.Core.Models;

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
