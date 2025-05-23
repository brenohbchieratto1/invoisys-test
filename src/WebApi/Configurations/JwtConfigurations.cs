namespace App.InvoiSysTest.WebApi.Configurations;

public static class JwtConfigurations
{
    public static readonly byte[] EncodedSecurityKey = "chave-secreta-supeeeeeeeeeeeeeeeeeeeeersegura"u8.ToArray();

    public const string InvoiSysTestRead = "invoisys-test-read";
    public const string InvoiSysTestWrite = "invoisys-test-write";
}