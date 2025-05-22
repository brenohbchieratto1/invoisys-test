using System.Diagnostics.CodeAnalysis;
using Strategyo.Results.Enumerations.SmartEnum;

namespace Strategyo.Results.Contracts.Results;

[ExcludeFromCodeCoverage]
public static class Messages
{
    public static Message Warning(string content) =>
        new(content, SmartMessageType.Warning);

    public static Message Information(string content) =>
        new(content, SmartMessageType.Information);

    public static Message Debug(string content) =>
        new(content, SmartMessageType.Debug);
}