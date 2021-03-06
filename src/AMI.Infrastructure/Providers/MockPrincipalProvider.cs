﻿using System;
using AMI.Core.Entities.Models;
using AMI.Core.Providers;

namespace AMI.Infrastructure.Providers
{
    /// <summary>
    /// A mock provider for custom principals.
    /// </summary>
    public class MockPrincipalProvider : ICustomPrincipalProvider
    {
        private readonly Guid sharedGuid = new Guid("11111111-1111-1111-1111-111111111111");

        /// <inheritdoc/>
        public ICustomPrincipal GetPrincipal()
        {
            return new MockPrincipal(sharedGuid.ToString());
        }
    }
}
