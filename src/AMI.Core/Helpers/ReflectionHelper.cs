using System;
using System.Reflection;
using AMI.Core.Extensions.StringExtensions;

namespace AMI.Core.Helpers
{
    internal static class ReflectionHelper
    {
        public static string GetAssemblyName()
        {
            var assembly = GetEntryOrCallingAssembly();
            return assembly?.GetName()?.Name;
        }

        public static string GetAssemblyName(Type type)
        {
            var assembly = Assembly.GetAssembly(type);
            return assembly?.GetName()?.Name;
        }

        public static string GetAssemblyVersion()
        {
            var assembly = GetEntryOrCallingAssembly();
            return assembly?.GetName()?.Name;
        }

        public static string GetAssemblyVersion(Type type)
        {
            var assembly = Assembly.GetAssembly(type);
            return assembly?.GetName()?.Version?.ToStringInvariant();
        }

        private static Assembly GetEntryOrCallingAssembly()
        {
            return Assembly.GetEntryAssembly() ?? Assembly.GetCallingAssembly();
        }
    }
}
