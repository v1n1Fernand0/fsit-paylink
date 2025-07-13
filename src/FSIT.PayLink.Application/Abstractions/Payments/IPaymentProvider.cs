namespace FSIT.PayLink.Application.Abstractions.Payments;

public interface IPaymentProvider
{
    Task<(string ProviderId, string? QrCodeUrl)> CreateChargeAsync(
        decimal amount,
        string currency,
        string cpf,              
        CancellationToken ct);
}
