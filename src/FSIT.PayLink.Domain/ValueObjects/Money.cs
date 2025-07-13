namespace FSIT.PayLink.Domain.ValueObjects;

/// <summary>Representa um valor monetário validado.</summary>
public sealed record Money
{
    public decimal Amount { get; }
    public string Currency { get; }

    private Money(decimal amount, string currency)
    {
        if (amount <= 0) throw new ArgumentOutOfRangeException(nameof(amount));
        if (currency.Length != 3) throw new Exceptions.InvalidCurrencyException(currency);

        Amount = decimal.Round(amount, 2, MidpointRounding.AwayFromZero);
        Currency = currency.ToUpperInvariant();
    }

    public static Money Of(decimal amount, string currency) => new(amount, currency);

    public override string ToString() => $"{Amount:F2} {Currency}";
}
