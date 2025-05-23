namespace App.InvoiSysTest.Infrastructure.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class DatabasePathAttribute(string filePatch) : Attribute
{
    public string FilePath { get; } = $"{filePatch}_db.txt";
}