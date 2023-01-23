
using ElasticAppSearch.ApiClient.Core;
using Microsoft.Extensions.Options;

namespace ElasticAppSearch.ApiClient.Services;

public class OptionsValidator: IValidateOptions<ElasticAppSearchOptions>
{
    public ValidateOptionsResult Validate(string? name, ElasticAppSearchOptions options)
    {
        var httpClientHandler = new HttpClientHandler();
        if (options.EnableHttpCompression.GetValueOrDefault() && !httpClientHandler.SupportsAutomaticDecompression)
        {
            return ValidateOptionsResult.Fail("Automatic HTTP response content compression isn't supported by platform");
        }
        return ValidateOptionsResult.Success;
    }
}
