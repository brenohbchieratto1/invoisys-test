using Strategyo.Enumerations.SmartEnum.Base;

namespace Strategyo.Results.Enumerations.SmartEnum;

public class SmartErrorType(int value, string name) : SmartEnum<SmartErrorType>(value, name)
{
    public static readonly SmartErrorType Failure = new(0, nameof(Failure));
    public static readonly SmartErrorType NotFound = new(1, nameof(NotFound));
    public static readonly SmartErrorType Validation = new(2, nameof(Validation));
    public static readonly SmartErrorType Conflict = new(3, nameof(Conflict));
    public static readonly SmartErrorType BadRequest = new(4, nameof(BadRequest));
    public static readonly SmartErrorType AccessUnAuthorized = new(5, nameof(AccessUnAuthorized));
    public static readonly SmartErrorType AccessForbidden = new(6, nameof(AccessForbidden));
}