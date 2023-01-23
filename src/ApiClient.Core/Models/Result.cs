using System.Collections.ObjectModel;

namespace ElasticAppSearch.ApiClient.Core.Models;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
public sealed record Result
{
    public ReadOnlyCollection<string> Errors { get; init; }

    public override string ToString()
    {
        return string.Join(Environment.NewLine, Errors);
    }
}
