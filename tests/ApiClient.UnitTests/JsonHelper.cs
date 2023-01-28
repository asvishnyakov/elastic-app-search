using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace ElasticAppSearch.ApiClient.UnitTests;

public static class JsonHelper
{
    public static string LoadFrom(string fileName)
    {
        var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
        using var fileReader = File.OpenText(path);
        using var jsonTextReader = new JsonTextReader(fileReader)
        {
            DateParseHandling = DateParseHandling.None
        };
        var jToken = JToken.ReadFrom(jsonTextReader);
        return jToken.ToString(Formatting.None);
    }
}