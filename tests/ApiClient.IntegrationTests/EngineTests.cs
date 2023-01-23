using ElasticAppSearch.ApiClient.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ElasticAppSearch.ApiClient.IntegrationTests;

public class EngineTests
{
    [Fact]
    public async Task GetEngine_Exists_ReturnEngineSuccessfully()
    {
        var serviceProvider = ServiceBuilder.GetServiceProvider();
        var apiClient = serviceProvider.GetRequiredService<IApiClient>();

        var engine = await apiClient.GetEngineAsync(Constants.EngineName);

        Assert.NotNull(engine);
    }
}