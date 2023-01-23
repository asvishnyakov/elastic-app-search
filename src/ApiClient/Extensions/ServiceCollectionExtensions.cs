using ElasticAppSearch.ApiClient.Services;
using Microsoft.Extensions.Options;
using System.Net;
using Microsoft.Net.Http.Headers;
using ElasticAppSearch.ApiClient.Core;

namespace ElasticAppSearch.ApiClient.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddElasticAppSearch(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IValidateOptions<ApiClientOptions>, OptionsValidator>();
        services.AddOptions<ApiClientOptions>().Bind(configuration).ValidateDataAnnotations().ValidateOnStart();

        services.AddSingleton<IApiClient, Services.ApiClient>();

        services.AddHttpClient(Configuration.HttpClientName, (serviceProvider, httpClient) =>
        {
            var elasticAppSearchOptions = serviceProvider.GetRequiredService<IOptions<ApiClientOptions>>().Value;

            httpClient.BaseAddress = new Uri($"{elasticAppSearchOptions.Endpoint}/api/as/v1/");

            httpClient.DefaultRequestHeaders.Add(HeaderNames.Authorization, $"Bearer {elasticAppSearchOptions.PrivateApiKey}");

            if (elasticAppSearchOptions.EnableHttpCompression.GetValueOrDefault())
            {
                httpClient.DefaultRequestHeaders.Add(HeaderNames.AcceptEncoding, DecompressionMethods.GZip.ToString());
            }
        }).ConfigurePrimaryHttpMessageHandler(serviceProvider =>
        {
            var elasticAppSearchOptions = serviceProvider.GetRequiredService<IOptions<ApiClientOptions>>().Value;

            var handler = new HttpClientHandler
            {
                AutomaticDecompression = elasticAppSearchOptions.EnableHttpCompression.GetValueOrDefault() ? DecompressionMethods.GZip : DecompressionMethods.None
            };

            return handler;
        });
    }
}