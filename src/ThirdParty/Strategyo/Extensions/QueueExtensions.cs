namespace Strategyo.Extensions;

public static class QueueExtensions
{
    public static string GetQueueNameFromType(this Type type)
        => type.Name.ToKebabCase();
    
    public static string GetSubscriberNameFromType(this Type type)
        => type.Name.ToKebabCase();
}