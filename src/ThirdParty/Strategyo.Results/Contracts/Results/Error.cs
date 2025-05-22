using System.Diagnostics.CodeAnalysis;
using Strategyo.Results.Enumerations.SmartEnum;

namespace Strategyo.Results.Contracts.Results;

[ExcludeFromCodeCoverage]
public class Error(string description, SmartErrorType errorType)
{
    public string Description { get; set; } = description;
    public int Code { get; set; } = errorType.Value;
    public string Type { get; set; } = errorType.Name;
}