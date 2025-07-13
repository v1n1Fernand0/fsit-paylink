namespace FSIT.PayLink.Domain.ValueObjects;

public sealed record Cpf(string Value)
{
    public static Cpf Of(string raw)
    {
        var digits = new string(raw.Where(char.IsDigit).ToArray());
        if (digits.Length != 11 || !IsValid(digits))
            throw new ArgumentException("CPF inválido.", nameof(raw));
        return new Cpf(digits);
    }

    private static bool IsValid(string d)
    {
        if (d.Distinct().Count() == 1) return false;
        int Calc(int len)
        {
            var sum = Enumerable.Range(0, len)
                        .Sum(i => (len + 1 - i) * (d[i] - '0'));
            var mod = (sum * 10) % 11;
            return mod == 10 ? 0 : mod;
        }
        return Calc(9) == d[9] - '0'
            && Calc(10) == d[10] - '0';
    }
}
