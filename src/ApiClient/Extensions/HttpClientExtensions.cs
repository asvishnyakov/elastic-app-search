using System.Text;
using ElasticAppSearch.ApiClient.Json.Constants;
using Newtonsoft.Json;

namespace ElasticAppSearch.ApiClient.Extensions;

internal static class HttpClientExtensions
{
    public static async Task<TResult?> GetFromJsonAsync<TResult>(this HttpClient client, string requestUri, CancellationToken cancellationToken = default)
    {
        var httpResponseMessage = await client.GetAsync(requestUri, cancellationToken);
        return await httpResponseMessage.JsonAsync<TResult>(cancellationToken);
    }

    public static async Task<TResult?> PostAsJsonAsync<TValue, TResult>(this HttpClient client, string requestUri, TValue value, CancellationToken cancellationToken = default)
    {
        var httpResponseMessage = await client.PostAsync(requestUri, value.ToJson(Defaults.JsonSerializerSettings), cancellationToken);
        return await httpResponseMessage.JsonAsync<TResult>(cancellationToken);
    }

    public static async Task<TResult?> PutAsJsonAsync<TValue, TResult>(this HttpClient client, string requestUri, TValue value, CancellationToken cancellationToken = default)
    {
        var httpResponseMessage = await client.PutAsync(requestUri, value.ToJson(Defaults.JsonSerializerSettings), cancellationToken);
        return await httpResponseMessage.JsonAsync<TResult>(cancellationToken);
    }

    public static async Task<TResult?> DeleteAsJsonAsync<TValue, TResult>(this HttpClient client, string requestUri, TValue value, CancellationToken cancellationToken = default)
    {
        var httpMessage = new HttpRequestMessage(HttpMethod.Delete, requestUri) { Content = value.ToJson(Defaults.JsonSerializerSettings) };
        var httpResponseMessage = await client.SendAsync(httpMessage, cancellationToken);
        return await httpResponseMessage.JsonAsync<TResult>(cancellationToken);
    }

    public static async Task<TValue?> JsonAsync<TValue>(this HttpResponseMessage httpResponseMessage, CancellationToken cancellationToken = default)
    {
        await httpResponseMessage.EnsureSuccessStatusCodeAsync<Result>(Defaults.JsonSerializerSettings, cancellationToken);
        return await httpResponseMessage.Content.ReadFromJsonAsync<TValue>(Defaults.JsonSerializerSettings, cancellationToken);
    }

    private static async Task EnsureSuccessStatusCodeAsync<TResult>(this HttpResponseMessage httpResponseMessage, JsonSerializerSettings? jsonSerializerSettings = null, CancellationToken cancellationToken = default)
    {
        try
        {
            httpResponseMessage.EnsureSuccessStatusCode();
        }
        catch (HttpRequestException exception)
        {
            var content = await httpResponseMessage.Content.ReadAsStringAsync(cancellationToken);
            var result = JsonConvert.DeserializeObject<TResult>(content, jsonSerializerSettings);
            throw new ApiException(result?.ToString(), exception);
        }
    }

    private static async Task<TValue?> ReadFromJsonAsync<TValue>(this HttpContent httpContent, JsonSerializerSettings? jsonSerializerSettings = null, CancellationToken cancellationToken = default)
    {
        var content = await httpContent.ReadAsStringAsync(cancellationToken);
        return JsonConvert.DeserializeObject<TValue>(content, jsonSerializerSettings);
    }

    private static HttpContent ToJson<TValue>(this TValue value, JsonSerializerSettings? jsonSerializerSettings = null)
    {
        var content = JsonConvert.SerializeObject(value, jsonSerializerSettings);
        return new StringContent(content, Encoding.UTF8, "application/json");
    }
}
