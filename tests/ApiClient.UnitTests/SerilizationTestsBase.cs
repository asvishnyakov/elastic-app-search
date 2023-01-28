using FluentAssertions;
using Newtonsoft.Json;

namespace ElasticAppSearch.ApiClient.UnitTests;

public class SerializationTestsBase
{
    public virtual void Serialize_Entity_CorrectlySerializes<T>(T actual, string expectedJsonFileName)
    {
        // Arrange
        var expectedJson = JsonHelper.LoadFrom(GetJsonPath(expectedJsonFileName));

        // Act
        var actualJson = JsonConvert.SerializeObject(actual, Defaults.JsonSerializerSettings);

        // Assert
        Assert.Equal(expectedJson, actualJson, ignoreLineEndingDifferences: true, ignoreWhiteSpaceDifferences: true);
    }

    protected void Serialize_InvalidData_ThrowsException<T>(T actual)
    {
        // Arrange
        void Serialize()
        {
            // Act
            JsonConvert.SerializeObject(actual, Defaults.JsonSerializerSettings);
        }

        // Assert
        Assert.Throws<JsonSerializationException>(Serialize);
    }

    public virtual void Deserialize_Json_CorrectlyDeserializes<T>(T expected, string actualJsonFileName)
    {
        // Arrange
        var actualJson = JsonHelper.LoadFrom(GetJsonPath(actualJsonFileName));

        // Act
        var actual = JsonConvert.DeserializeObject<T>(actualJson, Defaults.JsonSerializerSettings);

        // Assert
        actual.Should().BeEquivalentTo(expected, options => options.ComparingRecordsByMembers());
    }

    protected virtual string GetJsonPath()
    {
        return "Json";
    }

    protected virtual string GetJsonPath(string fileName)
    {
        return Path.Combine(GetJsonPath(), fileName);
    }
}