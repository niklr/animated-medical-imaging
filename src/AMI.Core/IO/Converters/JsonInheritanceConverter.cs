using System;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace AMI.Core.Converters
{
#pragma warning disable SA1201 // Elements must appear in the correct order
#pragma warning disable SA1204 // Static elements must appear before instance elements
#pragma warning disable SA1309 // Field names must not begin with underscore
#pragma warning disable SA1513 // Closing brace must be followed by blank line
    /// <summary>
    /// Defines the class as inheritance base class and adds a discriminator property to the serialized object.
    /// Source: https://raw.githubusercontent.com/RicoSuter/NJsonSchema/master/src/NJsonSchema/Converters/JsonInheritanceConverter.cs
    /// </summary>
    public class JsonInheritanceConverter : JsonConverter
    {
        /// <summary>Gets the default discriminiator name.</summary>
        public static string DefaultDiscriminatorName { get; } = "discriminator";

        private readonly Type _baseType;
        private readonly string _discriminator;
        private readonly bool _readTypeProperty;

        [ThreadStatic]
        private static bool _isReading;

        [ThreadStatic]
        private static bool _isWriting;

        /// <summary>Initializes a new instance of the <see cref="JsonInheritanceConverter"/> class.</summary>
        public JsonInheritanceConverter()
            : this(DefaultDiscriminatorName, false)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="JsonInheritanceConverter"/> class.</summary>
        /// <param name="discriminator">The discriminator.</param>
        public JsonInheritanceConverter(string discriminator)
            : this(discriminator, false)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="JsonInheritanceConverter"/> class.</summary>
        /// <param name="discriminator">The discriminator.</param>
        /// <param name="readTypeProperty">Read the $type property to determine the type (fallback).</param>
        public JsonInheritanceConverter(string discriminator, bool readTypeProperty)
        {
            _discriminator = discriminator;
            _readTypeProperty = readTypeProperty;
        }

        /// <summary>Initializes a new instance of the <see cref="JsonInheritanceConverter"/> class which only applies for the given base type.</summary>
        /// <remarks>Use this constructor for global registered converters (not defined on class).</remarks>
        /// <param name="baseType">The base type.</param>
        public JsonInheritanceConverter(Type baseType)
            : this(baseType, DefaultDiscriminatorName)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="JsonInheritanceConverter"/> class which only applies for the given base type.</summary>
        /// <remarks>Use this constructor for global registered converters (not defined on class).</remarks>
        /// <param name="baseType">The base type.</param>
        /// <param name="discriminator">The discriminator.</param>
        public JsonInheritanceConverter(Type baseType, string discriminator)
            : this(discriminator, false)
        {
            _baseType = baseType;
        }

        /// <summary>Gets the discriminator property name.</summary>
        public virtual string DiscriminatorName => _discriminator;

        /// <summary>Writes the JSON representation of the object.</summary>
        /// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter" /> to write to.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The calling serializer.</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            try
            {
                _isWriting = true;

                var jObject = JObject.FromObject(value, serializer);
                jObject[_discriminator] = JToken.FromObject(GetDiscriminatorValue(value.GetType()));
                writer.WriteToken(jObject.CreateReader());
            }
            finally
            {
                _isWriting = false;
            }
        }

        /// <summary>Gets a value indicating whether this <see cref="T:Newtonsoft.Json.JsonConverter" /> can write JSON.</summary>
        public override bool CanWrite
        {
            get
            {
                if (_isWriting)
                {
                    _isWriting = false;
                    return false;
                }

                return true;
            }
        }

        /// <summary>Gets a value indicating whether this <see cref="T:Newtonsoft.Json.JsonConverter" /> can read JSON.</summary>
        public override bool CanRead
        {
            get
            {
                if (_isReading)
                {
                    _isReading = false;
                    return false;
                }

                return true;
            }
        }

        /// <summary>Determines whether this instance can convert the specified object type.</summary>
        /// <param name="objectType">Type of the object.</param>
        /// <returns><c>true</c> if this instance can convert the specified object type; otherwise, <c>false</c>.</returns>
        public override bool CanConvert(Type objectType)
        {
            if (_baseType != null)
            {
                var type = objectType;
                while (type != null)
                {
                    if (type == _baseType)
                    {
                        return true;
                    }

                    type = type.GetTypeInfo().BaseType;
                }

                return false;
            }

            return true;
        }

        /// <summary>Reads the JSON representation of the object.</summary>
        /// <param name="reader">The <see cref="T:Newtonsoft.Json.JsonReader" /> to read from.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="existingValue">The existing value of object being read.</param>
        /// <param name="serializer">The calling serializer.</param>
        /// <returns>The object value.</returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jObject = serializer.Deserialize<JObject>(reader);
            if (jObject == null)
            {
                return null;
            }

            var discriminator = jObject.GetValue(_discriminator).Value<string>();
            var subtype = GetDiscriminatorType(jObject, objectType, discriminator);

            var objectContract = serializer.ContractResolver.ResolveContract(subtype) as JsonObjectContract;
            if (objectContract == null || objectContract.Properties.All(p => p.PropertyName != _discriminator))
            {
                jObject.Remove(_discriminator);
            }

            try
            {
                _isReading = true;
                return serializer.Deserialize(jObject.CreateReader(), subtype);
            }
            finally
            {
                _isReading = false;
            }
        }

        /// <summary>Gets the discriminator value for the given type.</summary>
        /// <param name="type">The object type.</param>
        /// <returns>The discriminator value.</returns>
        public virtual string GetDiscriminatorValue(Type type)
        {
            return type.Name;
        }

        /// <summary>Gets the type for the given discriminator value.</summary>
        /// <param name="jObject">The JSON object.</param>
        /// <param name="objectType">The object (base) type.</param>
        /// <param name="discriminatorValue">The discriminator value.</param>
        /// <returns>The type for the given discriminator value.</returns>
        protected virtual Type GetDiscriminatorType(JObject jObject, Type objectType, string discriminatorValue)
        {
            if (objectType.Name == discriminatorValue)
            {
                return objectType;
            }

            var knownTypeAttributesSubtype = GetSubtypeFromKnownTypeAttributes(objectType, discriminatorValue);
            if (knownTypeAttributesSubtype != null)
            {
                return knownTypeAttributesSubtype;
            }

            var typeName = objectType.Namespace + "." + discriminatorValue;
            var subtype = objectType.GetTypeInfo().Assembly.GetType(typeName);
            if (subtype != null)
            {
                return subtype;
            }

            if (_readTypeProperty)
            {
                var typeInfo = jObject.GetValue("$type");
                if (typeInfo != null)
                {
                    return Type.GetType(typeInfo.Value<string>());
                }
            }

            throw new InvalidOperationException("Could not find subtype of '" + objectType.Name + "' with discriminator '" + discriminatorValue + "'.");
        }

        private Type GetSubtypeFromKnownTypeAttributes(Type objectType, string discriminator)
        {
            var type = objectType;
            do
            {
                var knownTypeAttributes = type.GetTypeInfo().GetCustomAttributes(false)
                    .Where(a => a.GetType().Name == "KnownTypeAttribute");
                foreach (object attribute in knownTypeAttributes)
                {
                    var currentType = attribute.GetType();
                    if (currentType != null && currentType.Name == discriminator)
                    {
                        return attribute.GetType();
                    }
                    /*
                    else if (attribute.MethodName != null)
                    {
                        var method = type.GetRuntimeMethod((string)attribute.MethodName, new Type[0]);
                        if (method != null)
                        {
                            var types = (System.Collections.Generic.IEnumerable<Type>)method.Invoke(null, new object[0]);
                            foreach (var knownType in types)
                            {
                                if (knownType.Name == discriminator)
                                {
                                    return knownType;
                                }
                            }
                            return null;
                        }
                    }
                    */
                }

                type = type.GetTypeInfo().BaseType;
            }
            while (type != null);

            return null;
        }
    }
#pragma warning restore SA1201 // Elements must appear in the correct order
#pragma warning restore SA1204 // Static elements must appear before instance elements
#pragma warning restore SA1309 // Field names must not begin with underscore
#pragma warning restore SA1513 // Closing brace must be followed by blank line
}