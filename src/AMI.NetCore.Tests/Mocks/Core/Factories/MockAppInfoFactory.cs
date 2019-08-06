using System;
using System.Reflection;
using AMI.Core.Entities.Models;
using AMI.Core.Extensions.StringExtensions;
using AMI.Core.Factories;
using RNS.Framework.Semantic;

namespace AMI.NetCore.Tests.Mocks.Core.Factories
{
    internal class MockAppInfoFactory : IAppInfoFactory
    {
        public AppInfoModel Create()
        {
            return Create(Assembly.GetExecutingAssembly());
        }

        public AppInfoModel Create(Type type)
        {
            return Create(Assembly.GetAssembly(type));
        }

        private AppInfoModel Create(Assembly assembly)
        {
            var semVer = new SemanticVersion(assembly.GetName().Version.ToStringInvariant());
            return new AppInfoModel(assembly.GetName().Name, semVer.ToNormalizedString());
        }
    }
}
