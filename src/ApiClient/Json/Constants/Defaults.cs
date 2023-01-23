using ElasticAppSearch.ApiClient.Json.Resolvers;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace ElasticAppSearch.ApiClient.Json.Constants;

public static class Defaults
{
    public static readonly JsonSerializerSettings JsonSerializerSettings = new()
    {
        // Elastic App Search API use camelCase in JSON
        ContractResolver = new CustomContractResolver { NamingStrategy = new SnakeCaseNamingStrategy() },
        Converters = new List<JsonConverter>
        {
            new StringEnumConverter(new CamelCaseNamingStrategy())
        },

        // Elastic App Search API doesn't support fraction in seconds (probably bug in their ISO 8160 / RFC3399 specification support)
        DateFormatString = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.fffzzz",
        DateTimeZoneHandling = DateTimeZoneHandling.Utc,
    };
}