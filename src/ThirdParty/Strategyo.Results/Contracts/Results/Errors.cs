using System.Diagnostics.CodeAnalysis;
using Strategyo.Results.Enumerations.SmartEnum;

namespace Strategyo.Results.Contracts.Results;

[ExcludeFromCodeCoverage]
public static class Errors
{
    public static Error Failure(string message) =>
        new(message, SmartErrorType.Failure);

    public static Error NotFound(string message) =>
        new(message, SmartErrorType.NotFound);

    public static Error Validation(string message) =>
        new(message, SmartErrorType.Validation);

    public static Error Conflict(string message) =>
        new(message, SmartErrorType.Conflict);

    public static Error BadRequest(string message) =>
        new(message, SmartErrorType.BadRequest);

    public static Error AccessUnAuthorized(string message) =>
        new(message, SmartErrorType.AccessUnAuthorized);

    public static Error AccessForbidden(string message) =>
        new(message, SmartErrorType.AccessForbidden);
}