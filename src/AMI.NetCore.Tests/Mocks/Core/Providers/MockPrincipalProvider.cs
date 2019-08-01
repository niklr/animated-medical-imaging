using System;
using AMI.Core.Entities.Models;
using AMI.Core.Providers;
using NUnit.Framework.Internal;

namespace AMI.NetCore.Tests.Mocks.Core.Providers
{
    public class MockPrincipalProvider : ICustomPrincipalProvider
    {
        private readonly Guid sharedGuid = new Guid("11111111-1111-1111-1111-111111111111");

        /// <inheritdoc/>
        public ICustomPrincipal GetPrincipal()
        {
            var principal = TestExecutionContext.CurrentContext?.CurrentPrincipal;
            if (principal is ICustomPrincipal customPrincipal)
            {
                return customPrincipal;
            }
            return new MockPrincipal(sharedGuid.ToString());
        }
    }
}
