using System;
using System.Globalization;

namespace AMI.Core.Extensions.StringExtensions
{
    /// <summary>
    /// Extensions related to strings.
    /// </summary>
    internal static class StringExtensions
    {
        /// <summary>
        /// Converts the value of the specified object to its equivalent string representation
        /// using the invariant culture.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="value">An object that supplies the value to convert, or null.</param>
        /// <returns>The string representation of value, or System.String.Empty if value is an object
        /// whose value is null. If value is null, the method returns null.</returns>
        public static string ToStringInvariant<T>(this T value)
        {
            return Convert.ToString(value, CultureInfo.InvariantCulture);
        }
    }
}
