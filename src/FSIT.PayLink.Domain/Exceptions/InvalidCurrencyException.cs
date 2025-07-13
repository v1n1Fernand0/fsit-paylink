namespace FSIT.PayLink.Domain.Exceptions;

public sealed class InvalidCurrencyException : Exception
{
    public InvalidCurrencyException(string currency)
        : base($"Currency '{currency}' is invalid. Use ISO-4217, ex.: 'BRL', 'USD'.") { }
}
