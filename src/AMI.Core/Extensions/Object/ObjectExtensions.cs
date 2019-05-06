using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace AMI.Core.Extensions.ObjectExtensions
{
    /// <summary>
    /// Extensions related to objects.
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// Creates a deep clone of the provided object.
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <param name="o">The object to clone.</param>
        /// <returns>A deep clone of the object.</returns>
        public static T DeepClone<T>(this T o)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, o);
                stream.Position = 0;
                return (T)formatter.Deserialize(stream);
            }
        }
    }
}
