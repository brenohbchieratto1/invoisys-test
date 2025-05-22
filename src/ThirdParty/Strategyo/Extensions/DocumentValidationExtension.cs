namespace Strategyo.Extensions;

public static class DocumentValidationExtension
{
    public static bool ValidateCpf(this string cpf)
    {
        cpf = cpf.Trim().Replace(".", "").Replace("-", "");
        
        if (cpf.Length != 11 || !cpf.All(char.IsDigit))
        {
            return false;
        }
        
        int[] factors1 = [10, 9, 8, 7, 6, 5, 4, 3, 2];
        int[] factors2 = [11, 10, 9, 8, 7, 6, 5, 4, 3, 2];
        
        var part1 = cpf[..9];
        var sum1 = 0;
        var sum2 = 0;
        
        for (var i = 0; i < 9; i++)
        {
            sum1 += (int)char.GetNumericValue(part1[i]) * factors1[i];
            sum2 += (int)char.GetNumericValue(part1[i]) * factors2[i];
        }
        
        var digit1 = 11 - sum1 % 11;
        digit1 = digit1 >= 10 ? 0 : digit1;
        
        sum2 += digit1 * factors2[9];
        var digit2 = 11 - sum2 % 11;
        digit2 = digit2 >= 10 ? 0 : digit2;
        
        return cpf[9..] == $"{digit1}{digit2}";
    }
    
    public static bool ValidateCnpj(this string cnpj)
    {
        cnpj = cnpj.Trim().Replace(".", "").Replace("/", "").Replace("-", "");
        
        if (cnpj.Length != 14 || !cnpj.All(char.IsDigit))
        {
            return false;
        }
        
        int[] factors1 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
        int[] factors2 = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
        
        var part1 = cnpj[..12];
        var sum1 = 0;
        var sum2 = 0;
        
        for (var i = 0; i < 12; i++)
        {
            sum1 += (int)char.GetNumericValue(part1[i]) * factors1[i];
            sum2 += (int)char.GetNumericValue(part1[i]) * factors2[i];
        }
        
        var digit1 = 11 - sum1 % 11;
        digit1 = digit1 >= 10 ? 0 : digit1;
        
        sum2 += digit1 * factors2[12];
        var digit2 = 11 - sum2 % 11;
        digit2 = digit2 >= 10 ? 0 : digit2;
        
        return cnpj[12..] == $"{digit1}{digit2}";
    }
}