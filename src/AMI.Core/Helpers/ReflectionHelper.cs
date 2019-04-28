using System.Reflection;
using AMI.Core.Extensions.StringExtensions;

namespace AMI.Core.Helpers
{
    internal static class ReflectionHelper
    {
        public static string GetAssemblyName()
        {
            var assembly = GetExecutingOrEntryAssembly();
            return assembly.GetName().Name;
        }

        public static string GetAssemblyVersion()
        {
            var assembly = GetExecutingOrEntryAssembly();
            return assembly.GetName().Version.ToStringInvariant();
        }

        private static Assembly GetExecutingOrEntryAssembly()
        {
            return Assembly.GetEntryAssembly() ?? Assembly.GetCallingAssembly();
        }
    }
}
