namespace ElasticAppSearch.ApiClient.Core.Constants;

public static class Endpoints
{
    public const string Engines = "engines";

    public static string GetEngineEndpoint(string engineName)
    {
        return $"{Engines}/{engineName}";
    }
}