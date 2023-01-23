using System.Collections;
using ElasticAppSearch.ApiClient.Extensions;
using ElasticAppSearch.ApiClient.Json.Handling;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ElasticAppSearch.ApiClient.Json.Converters
{
    public class ArrayConverter : JsonConverter
    {
        private readonly SingleValueHandling _singleValueHandling;
        private readonly JsonConverter _itemConverter;

        public ArrayConverter() : this(SingleValueHandling.AsArray)
        {
        }

        public ArrayConverter(SingleValueHandling singleValueHandling) : this(singleValueHandling, null)
        {
        }

        public ArrayConverter(SingleValueHandling singleValueHandling, Type? itemConverterType) : this(singleValueHandling, itemConverterType, null)
        {
        }

        public ArrayConverter(SingleValueHandling singleValueHandling, Type? itemConverterType, object?[]? itemConverterParameters)
        {
            _singleValueHandling = singleValueHandling;
            _itemConverter = itemConverterType != null
                ? (JsonConverter)Activator.CreateInstance(itemConverterType, itemConverterParameters)!
                : new DefaultJsonConverter<object>();
        }

        public override bool CanConvert(Type objectType)
        {
            var result = objectType.IsAssignableFrom(typeof(IEnumerable)) && _itemConverter.CanConvert(objectType.GetEnumerableElementType());
            return result;
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            var token = JToken.Load(reader);

            if (token.Type != JTokenType.Null)
            {
                if (_singleValueHandling == SingleValueHandling.AsArray || token.Type == JTokenType.Array)
                {
                    return Deserialize(token, objectType, existingValue, serializer);
                }

                var elementType = objectType.GetEnumerableElementType();
                return Deserialize(token, elementType, existingValue, serializer);
            }

            return token.ToObject(objectType, serializer);
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            if (value != null)
            {
                var array = value.AsArray();
                if (_singleValueHandling == SingleValueHandling.AsArray || array?.Length > 1)
                {
                    Serialize(writer, array, serializer);
                }
                else
                {
                    var element = array?.FirstOrDefault();
                    Serialize(writer, element, serializer);
                }
            }
            else
            {
                serializer.Serialize(writer, null);
            }
        }

        private object? Deserialize(JToken token, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            var list = new List<object?>();
            if (token.Type == JTokenType.Array)
            {
                var elementType = objectType.GetEnumerableElementType();
                list.AddRange(((JArray)token).Select(elementToken => _itemConverter.ReadJson(elementToken.CreateReader(), elementType, null, serializer)));
            }
            else
            {
                list.Add(_itemConverter.ReadJson(token.CreateReader(), objectType, existingValue, serializer));
            }

            var result = serializer.Deserialize(JToken.FromObject(list, serializer).CreateReader(), objectType);
            return result;
        }

        private void Serialize(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            if (value is Array array)
            {
                writer.WriteStartArray();
                foreach (var element in array)
                {
                    _itemConverter.WriteJson(writer, element, serializer);
                }
                writer.WriteEndArray();
            }
            else
            {
                _itemConverter.WriteJson(writer, value, serializer);
            }
        }
    }
}
