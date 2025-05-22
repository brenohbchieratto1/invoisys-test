using Strategyo.Enumerations.SmartEnum.Base;

namespace Strategyo.Results.Enumerations.SmartEnum;

public class SmartMessageType(int value, string name) : SmartEnum<SmartMessageType>(value, name)
{
    public static readonly SmartMessageType Warning = new(0, nameof(Warning));
    public static readonly SmartMessageType Information = new(1, nameof(Information));
    public static readonly SmartMessageType Debug = new(2, nameof(Debug));
}