using System.ComponentModel.DataAnnotations;

namespace ElasticAppSearch.ApiClient.Core;

public sealed record ApiClientOptions
{
    [Required]
    public string Endpoint { get; init; } = null!;

    [Required]
    public string PrivateApiKey { get; init; } = null!;

    public bool? EnableHttpCompression { get; init; }
}