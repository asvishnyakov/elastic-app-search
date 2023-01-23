using ElasticAppSearch.ApiClient.Core.Constants;

namespace ElasticAppSearch.ApiClient.Core.Attributes;

public class MaximumVersionAttribute : VersionAttribute
{
    public MaximumVersionAttribute(int major, int minor): base(major, minor)
    {
        if (Version < Versions.Supported.Minimum)
        {
            throw new ArgumentException($"API client supports Elastic App Search starting from {Versions.Supported.Minimum}");
        }
    }
}