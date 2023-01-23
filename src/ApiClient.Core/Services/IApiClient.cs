using ElasticAppSearch.ApiClient.Core.Models.Engines;

namespace ElasticAppSearch.ApiClient.Core.Services;

public interface IApiClient
{
    Task<Engine?> GetEngineAsync(string name, CancellationToken cancellationToken = default);
}