using System.Linq;
using AMI.Core.Extensions.StringExtensions;
using RNS.Framework.Tools;

namespace AMI.Core.Extensions.ArrayExtensions
{
    /// <summary>
    /// Extensions related to arrays.
    /// </summary>
    public static class ArrayExtensions
    {
        /// <summary>
        /// Concatenates the members of the array of type <see cref="string"/>, using the first separator between each member.
        /// Additionally, each member is embedded between the second separator.
        /// </summary>
        /// <param name="values">A collection that contains the strings to concatenate.</param>
        /// <param name="separator1">The first separator.</param>
        /// <param name="separator2">The second separator.</param>
        /// <returns>
        /// A string that consists of the members of values delimited by the first separator string
        /// and embedded between the second separator string.
        /// If values has no members, the method returns <see cref="string.Empty"/>.
        /// </returns>
        public static string ToString(this string[] values, string separator1, string separator2)
        {
            Ensure.ArgumentNotNull(values, nameof(values));
            Ensure.ArgumentNotNullOrWhiteSpace(separator1, nameof(separator1));
            Ensure.ArgumentNotNullOrWhiteSpace(separator2, nameof(separator2));

            return string.Join(separator1, values.Select(x => x.Embed(separator2)));
        }
    }
}
