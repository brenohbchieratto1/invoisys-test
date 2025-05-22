namespace App.InvoisysTest.Infrastructure.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class DatabasePathAttribute(string filePatch) : Attribute
{
    public string FilePath { get; } = $"{filePatch}.txt";
}