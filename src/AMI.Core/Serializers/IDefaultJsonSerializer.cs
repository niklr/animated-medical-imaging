using Newtonsoft.Json;

namespace AMI.Core.Serializers
{
    /// <summary>
    /// A serializer for JSON data.
    /// </summary>
    public interface IDefaultJsonSerializer
    {
        /// <summary>
        /// Overrides the provided JSON serializer settings with the default settings.
        /// </summary>
        /// <param name="settings">The settings.</param>
        void OverrideJsonSerializerSettings(JsonSerializerSettings settings);

        /// <summary>
        /// Serializes the specified object.
        /// </summary>
        /// <param name="value">The object.</param>
        /// <returns>The serialized object.</returns>
        string Serialize(object value);

        /// <summary>
        /// Deserializes the specified JSON string.
        /// </summary>
        /// <typeparam name="T">The type of the deserialized object.</typeparam>
        /// <param name="json">The JSON string.</param>
        /// <returns>The deserialized JSON string.</returns>
        T Deserialize<T>(string json);
    }
}
