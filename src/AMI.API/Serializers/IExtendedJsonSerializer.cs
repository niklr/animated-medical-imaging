using AMI.Core.Serializers;
using Newtonsoft.Json;

namespace AMI.API.Serializers
{
    /// <summary>
    /// An extended serializer for JSON data.
    /// </summary>
    /// <seealso cref="IDefaultJsonSerializer" />
    public interface IExtendedJsonSerializer : IDefaultJsonSerializer
    {
        /// <summary>
        /// Sets the JSON serializer settings.
        /// </summary>
        /// <param name="settings">The settings.</param>
        void SetJsonSerializerSettings(JsonSerializerSettings settings);
    }
}
