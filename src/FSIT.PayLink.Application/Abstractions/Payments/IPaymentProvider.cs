namespace FSIT.PayLink.Application.Abstractions.Payments;

/// <summary>Gateway externo (Stripe, Abacate Pix etc.).</summary>
public interface IPaymentProvider
{
    Task<(string ProviderId, string? QrCodeUrl)>
        CreateChargeAsync(decimal amount, string currency, CancellationToken ct);
}
