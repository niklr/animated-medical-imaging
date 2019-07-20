using System;
using AMI.Core.IO.Serializers;
using JWT;

namespace AMI.Infrastructure.Wrappers
{
    /// <summary>
    /// A wrapper for the JSON serializer of the JWT package.
    /// </summary>
    /// <seealso cref="JWT.IJsonSerializer" />
    public class JwtJsonSerializerWrapper : IJsonSerializer
    {
        private readonly IDefaultJsonSerializer serializer;

        /// <summary>
        /// Initializes a new instance of the <see cref="JwtJsonSerializerWrapper"/> class.
        /// </summary>
        /// <param name="serializer">The serializer.</param>
        /// <exception cref="ArgumentNullException">serializer</exception>
        public JwtJsonSerializerWrapper(IDefaultJsonSerializer serializer)
        {
            this.serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
        }

        /// <inheritdoc/>
        public T Deserialize<T>(string json)
        {
            return serializer.Deserialize<T>(json);
        }

        /// <inheritdoc/>
        public string Serialize(object obj)
        {
            return serializer.Serialize(obj);
        }
    }
}
