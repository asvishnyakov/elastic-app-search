using Newtonsoft.Json;

namespace ElasticAppSearch.ApiClient.Json.Converters;

public class DefaultJsonConverter<T> : JsonConverter<T>
{
    // _isReading, _isWriting fields are thread-static and type-specific,
    // Undesirable behavior (values are not shared between types with different generic arguments) is correct behavior in this case.

    [ThreadStatic]
    private static bool _isReading;

    [ThreadStatic]
    private static bool _isWriting;

    public override bool CanRead
    {
        get
        {
            if (!_isReading)
            {
                return true;
            }

            _isReading = false;

            return false;
        }
    }

    public override bool CanWrite
    {
        get
        {
            if (!_isWriting)
            {
                return true;
            }

            _isWriting = false;

            return false;
        }
    }

    public override T? ReadJson(JsonReader reader, Type objectType, T? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        try
        {
            _isReading = true;
            return hasExistingValue ? existingValue : (T?)serializer.Deserialize(reader, objectType);
        }
        finally
        {
            _isReading = false;
        }
    }

    public override void WriteJson(JsonWriter writer, T? value, JsonSerializer serializer)
    {
        try
        {
            _isWriting = true;

            serializer.Serialize(writer, value);
        }
        finally
        {
            _isWriting = false;
        }
    }
}
