using System;
using System.Reflection;
using AMI.Core.Extensions.StringExtensions;

namespace AMI.Core.Helpers
{
    internal static class ReflectionHelper
    {
        public static string GetAssemblyName()
        {
            return GetAssemblyName(GetEntryOrCallingAssembly());
        }

        public static string GetAssemblyName(Type type)
        {
            return GetAssemblyName(Assembly.GetAssembly(type));
        }

        public static string GetAssemblyVersion()
        {
            return GetAssemblyVersion(GetEntryOrCallingAssembly());
        }

        public static string GetAssemblyVersion(Type type)
        {
            return GetAssemblyVersion(Assembly.GetAssembly(type));
        }

        private static string GetAssemblyName(Assembly assembly)
        {
            return assembly?.GetName()?.Name;
        }

        private static string GetAssemblyVersion(Assembly assembly)
        {
            return assembly?.GetName()?.Version?.ToStringInvariant();
        }

        private static Assembly GetEntryOrCallingAssembly()
        {
            return Assembly.GetEntryAssembly() ?? Assembly.GetCallingAssembly();
        }
    }
}
