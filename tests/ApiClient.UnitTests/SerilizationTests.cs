using Newtonsoft.Json;

namespace ElasticAppSearch.ApiClient.UnitTests;

public class SerializationTests : SerializationTestsBase
{
    public static readonly IEnumerable<object[]> WriteJsonData = new List<object[]>
    {
        new object[] { new IncludeEmptyString { Property = string.Empty }, "PropertyEmptyString.json" },
        new object[] { new IncludeEmptyString { Property = "test" }, "PropertyValue.json" },
        new object[] { new IgnoreEmptyString { Property = string.Empty }, "Empty.json" },
        new object[] { new IgnoreEmptyString { Property = "test" }, "PropertyValue.json" },
        
        new object[] { new IncludeEmptyArray { Property = Array.Empty<string>() }, "PropertyEmptyArray.json" },
        new object[] { new IncludeEmptyArray { Property = new[] { "test" } }, "PropertyArrayValue.json" },
        new object[] { new IgnoreEmptyArray { Property = Array.Empty<string>() }, "Empty.json" },
        new object[] { new IgnoreEmptyArray { Property = new[] { "test" } }, "PropertyArrayValue.json" },
        
        new object[] { new SingleValueAsArray { Property = new[] { "test" } }, "PropertyArrayValue.json" },
        new object[] { new SingleValueAsArray { Property = new[] { "test1", "test2" } }, "PropertyArrayValues.json" },
        new object[] { new SingleValueAsObject { Property = new[] { "test" } }, "PropertyValue.json" },
        new object[] { new SingleValueAsObject { Property = new[] { "test1", "test2" } }, "PropertyArrayValues.json" },

        new object[] { new SingleObjectAsArray { Property = new[] { new Test("test") } }, "PropertyArrayValue.json" },
        new object[] { new SingleObjectAsArray { Property = new[] { new Test("test1"), new Test("test2") } }, "PropertyArrayValues.json" },
        new object[] { new SingleObjectAsObject { Property = new[] { new Test("test") } }, "PropertyValue.json" },
        new object[] { new SingleObjectAsObject { Property = new[] { new Test("test1"), new Test("test2") } }, "PropertyArrayValues.json" },
    };

    [Theory]
    [MemberData(nameof(WriteJsonData))]
    public override void Serialize_Entity_CorrectlySerializes<T>(T actual, string expectedJsonFileName)
    {
        base.Serialize_Entity_CorrectlySerializes(actual, expectedJsonFileName);
    }

    private record IncludeEmptyString
    {
        [CustomJsonProperty(EmptyValueHandling = EmptyValueHandling.Include)]
        public string Property { get; init; } = null!;
    }

    private record IgnoreEmptyString
    {
        [CustomJsonProperty(EmptyValueHandling = EmptyValueHandling.Ignore)]
        public string Property { get; init; } = null!;
    }

    private record IncludeEmptyArray
    {
        [CustomJsonProperty(EmptyValueHandling = EmptyValueHandling.Include)]
        public string[] Property { get; init; } = null!;
    }
    
    private record IgnoreEmptyArray
    {
        [CustomJsonProperty(EmptyValueHandling = EmptyValueHandling.Ignore)]
        public string[] Property { get; init; } = null!;
    }

    private record SingleValueAsArray
    {
        [JsonConverter(typeof(CollectionConverter), SingleValueHandling.AsArray)]
        public string[] Property { get; init; } = null!;
    }

    private record SingleValueAsObject
    {
        [JsonConverter(typeof(CollectionConverter), SingleValueHandling.AsObject)]
        public string[] Property { get; init; } = null!;
    }

    private record SingleObjectAsArray
    {
        [JsonConverter(typeof(CollectionConverter), SingleValueHandling.AsArray, typeof(TestConverter))]
        public Test[] Property { get; init; } = null!;
    }

    private record SingleObjectAsObject
    {
        [JsonConverter(typeof(CollectionConverter), SingleValueHandling.AsObject, typeof(TestConverter))]
        public Test[] Property { get; init; } = null!;
    }

    private record Test(string Property);

    private class TestConverter : JsonConverter<Test>
    {
        public override Test? ReadJson(JsonReader reader, Type objectType, Test? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var property = serializer.Deserialize<string>(reader);
            return property != null ? new Test(property) : null;
        }

        public override void WriteJson(JsonWriter writer, Test? value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value?.Property, typeof(string));
        }
    }
}