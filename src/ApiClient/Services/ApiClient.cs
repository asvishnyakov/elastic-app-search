using ElasticAppSearch.ApiClient.Extensions;

namespace ElasticAppSearch.ApiClient.Services;

public class ApiClient: IApiClient
{
    private readonly HttpClient _httpClient;

    public ApiClient(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient(Configuration.HttpClientName);
    }

    public async Task<Engine?> GetEngineAsync(string name, CancellationToken cancellationToken = default)
    {
        return await _httpClient.GetFromJsonAsync<Engine>(Endpoints.GetEngineEndpoint(name), cancellationToken);
    }
}