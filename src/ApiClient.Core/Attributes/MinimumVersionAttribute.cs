using ElasticAppSearch.ApiClient.Core.Constants;

namespace ElasticAppSearch.ApiClient.Core.Attributes;

public class MinimumVersionAttribute : VersionAttribute
{
    public MinimumVersionAttribute(int major, int minor): base(major, minor)
    {
        if (Version > Versions.Supported.Maximum)
        {
            throw new ArgumentException($"API client supports Elastic App Search up to {Versions.Supported.Maximum}");
        }
    }
}