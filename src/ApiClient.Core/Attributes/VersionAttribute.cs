namespace ElasticAppSearch.ApiClient.Core.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Enum | AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Struct)]
public abstract class VersionAttribute: Attribute
{
    public Version Version { get; }

    protected VersionAttribute(int major, int minor)
    {
        Version = new Version(major, minor);
    }
}