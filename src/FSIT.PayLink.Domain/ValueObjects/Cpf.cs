using System;
using System.Linq;

namespace FSIT.PayLink.Domain.ValueObjects;

/// <summary>CPF validado (11 dígitos e dígitos verificadores).</summary>
public sealed record Cpf
{
    public string Value { get; }
    private Cpf(string v) => Value = v;

    public static Cpf Of(string raw)
    {
        var d = new string(raw.Where(char.IsDigit).ToArray());
        if (d.Length != 11 || !IsValid(d))
            throw new ArgumentException("CPF inválido.", nameof(raw));
        return new Cpf(d);
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

    public override string ToString() => Value;
}
