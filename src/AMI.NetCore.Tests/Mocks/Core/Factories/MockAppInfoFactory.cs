using System.Reflection;
using AMI.Core.Entities.Models;
using AMI.Core.Extensions.StringExtensions;
using AMI.Core.Factories;
using RNS.Framework.Semantic;

namespace AMI.NetCore.Tests.Mocks.Core.Factories
{
    internal class MockAppInfoFactory : IAppInfoFactory
    {
        public AppInfo Create()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var semVer = new SemanticVersion(assembly.GetName().Version.ToStringInvariant());
            return new AppInfo(assembly.GetName().Name, semVer.ToNormalizedString());
        }
    }
}
