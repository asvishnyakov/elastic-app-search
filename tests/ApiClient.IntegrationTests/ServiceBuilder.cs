using ElasticAppSearch.ApiClient.Core;
using ElasticAppSearch.ApiClient.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ElasticAppSearch.ApiClient.IntegrationTests;

public static class ServiceBuilder
{
    public static ServiceCollection GetServiceCollection()
    {
        var configurationBuilder = new ConfigurationBuilder();
        // Configure default values
        configurationBuilder.AddInMemoryCollection(new Dictionary<string, string?>
        {
            { $"{nameof(ElasticAppSearch)}:{nameof(ApiClientOptions.Endpoint)}", Constants.DefaultEndpoint }
        });
        // Load secrets and overriding values from environment variables
        configurationBuilder.AddEnvironmentVariables();
        var configuration = configurationBuilder.Build();

        var services = new ServiceCollection();
        services.AddElasticAppSearch(configuration.GetSection(nameof(ElasticAppSearch)));
        return services;
    }

    public static IServiceProvider GetServiceProvider()
    {
        var services = GetServiceCollection();
        return services.BuildServiceProvider();
    }
}