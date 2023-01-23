namespace ElasticAppSearch.ApiClient.Extensions;

public static class TypeExtensions
{
    public static Type GetEnumerableElementType(this Type type)
    {
        if (type.HasElementType)
        {
            return type.GetElementType();
        }

        if (type.IsGenericType)
        {
            return type.GenericTypeArguments.First();
        }

        return typeof(object);
    }
}
