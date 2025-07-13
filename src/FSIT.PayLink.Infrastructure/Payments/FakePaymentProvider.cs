using FSIT.PayLink.Application.Abstractions.Payments;   

namespace FSIT.PayLink.Infrastructure.Payments;

/// <summary>Provider simulado que gera IDs e QR Codes de teste.</summary>
public class FakePaymentProvider : IPaymentProvider         
{
    public Task<(string ProviderId, string? QrCodeUrl)>
        CreateChargeAsync(decimal amount, string currency, CancellationToken ct)
    {
        var id = Guid.NewGuid().ToString("N");
        var qr = $"https://fake.qr/{id}";
        return Task.FromResult((id, qr));
    }
}
