using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace AMI.API.Serializers
{
    /// <summary>
    /// An extended serializer for JSON data.
    /// </summary>
    /// <seealso cref="IExtendedJsonSerializer" />
    public class ExtendedJsonSerializer : IExtendedJsonSerializer
    {
        private readonly JsonSerializerSettings settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtendedJsonSerializer"/> class.
        /// </summary>
        public ExtendedJsonSerializer()
        {
            settings = new JsonSerializerSettings();
            SetJsonSerializerSettings(settings);
        }

        /// <summary>
        /// Sets the JSON serializer settings.
        /// </summary>
        /// <param name="settings">The settings.</param>
        public void SetJsonSerializerSettings(JsonSerializerSettings settings)
        {
            // settings.NullValueHandling = NullValueHandling.Include;
            settings.Formatting = Formatting.Indented;
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }

        /// <summary>
        /// Serializes the specified object.
        /// </summary>
        /// <param name="value">The object.</param>
        /// <returns>
        /// The serialized object.
        /// </returns>
        public string Serialize(object value)
        {
            return JsonConvert.SerializeObject(value, settings);
        }

        /// <summary>
        /// Deserializes the specified JSON string.
        /// </summary>
        /// <typeparam name="T">The type of the deserialized object.</typeparam>
        /// <param name="json">The JSON string.</param>
        /// <returns>The deserialized JSON string.</returns>
        public T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
