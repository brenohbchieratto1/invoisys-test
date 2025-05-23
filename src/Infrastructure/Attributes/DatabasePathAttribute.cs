using System.Diagnostics.CodeAnalysis;

namespace App.InvoiSysTest.Infrastructure.Attributes;

[ExcludeFromCodeCoverage]
[AttributeUsage(AttributeTargets.Class)]
public class DatabasePathAttribute(string filePatch) : Attribute
{
    public string FilePath { get; } = $"{filePatch}_db.txt";
}