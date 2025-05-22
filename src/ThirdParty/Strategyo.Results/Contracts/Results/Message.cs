using Strategyo.Results.Enumerations.SmartEnum;

namespace Strategyo.Results.Contracts.Results;

public class Message(string description, SmartMessageType messageType)
{
    public string Description { get; set; } = description;
    public int Code { get; set; } = messageType.Value;
    public string Type { get; set; } = messageType.Name;
}