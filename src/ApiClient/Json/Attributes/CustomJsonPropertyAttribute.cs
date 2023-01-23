using ElasticAppSearch.ApiClient.Json.Handling;

namespace ElasticAppSearch.ApiClient.Json.Attributes;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Parameter)]
public class CustomJsonPropertyAttribute : Attribute
{
    public EmptyValueHandling EmptyValueHandling { get; set; }
}
