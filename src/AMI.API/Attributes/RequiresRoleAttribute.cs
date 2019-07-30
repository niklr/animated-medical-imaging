using System;
using AMI.API.Requirements;
using AMI.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace AMI.API.Attributes
{
    /// <summary>
    /// Specifies that the class or method that this attribute is applied to requires the specified role-based authorization.
    /// </summary>
    /// <example>
    /// Example OR:
    /// [RequiresRole(Permission.User, Permission.Manager)]
    /// public class TestController : Controller
    ///
    /// Example AND:
    /// [RequiresRole(Permission.User)]
    /// [RequiresRole(Permission.Manager)]
    /// public class TestController : Controller
    /// </example>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class RequiresRoleAttribute : TypeFilterAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RequiresRoleAttribute"/> class.
        /// </summary>
        /// <param name="roles">The roles.</param>
        public RequiresRoleAttribute(params RoleType[] roles)
             : base(typeof(RequiresRoleFilterAttribute))
        {
            Arguments = new[] { new RoleAuthorizationRequirement(roles) };
        }
    }
}
