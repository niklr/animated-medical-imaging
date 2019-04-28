using System;
using System.Globalization;

namespace AMI.Core.Extensions.StringExtensions
{
    internal static class StringExtensions
    {
        public static string ToStringInvariant<T>(this T value)
        {
            return Convert.ToString(value, CultureInfo.InvariantCulture);
        }
    }
}
