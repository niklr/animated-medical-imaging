using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace AMI.Core.Serializers
{
    /// <summary>
    /// A serializer for JSON data.
    /// </summary>
    /// <seealso cref="IDefaultJsonSerializer" />
    public class DefaultJsonSerializer : IDefaultJsonSerializer
    {
        private readonly JsonSerializerSettings settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultJsonSerializer"/> class.
        /// </summary>
        public DefaultJsonSerializer()
        {
            settings = new JsonSerializerSettings();
            OverrideJsonSerializerSettings(settings);
        }

        /// <summary>
        /// Overrides the provided JSON serializer settings with the default settings.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <exception cref="ArgumentNullException">settings</exception>
        public void OverrideJsonSerializerSettings(JsonSerializerSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            settings.Formatting = Formatting.Indented;
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            settings.Converters.Add(new StringEnumConverter());
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
