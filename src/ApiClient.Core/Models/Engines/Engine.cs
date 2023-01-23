using ElasticAppSearch.ApiClient.Core.Attributes;

namespace ElasticAppSearch.ApiClient.Core.Models.Engines;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
public sealed record Engine
{
    public string Name { get; init; }

    public EngineType Type { get; init; }

    public string Language { get; init; }

    [MinimumVersion(7, 11)]
    public int DocumentCount { get; init; }
}