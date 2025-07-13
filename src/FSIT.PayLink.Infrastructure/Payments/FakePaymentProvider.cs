using FSIT.PayLink.Application.Abstractions.Payments;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FSIT.PayLink.Infrastructure.Payments;

public class FakePaymentProvider : IPaymentProvider
{
    public Task<(string ProviderId, string? QrCodeUrl)> CreateChargeAsync(
        decimal amount,
        string currency,
        string cpf,                  
        CancellationToken ct)
    {
        var fakeId = Guid.NewGuid().ToString();
        var fakeQr = $"https://fake.qr/{fakeId}";
        return Task.FromResult<(string, string?)>((fakeId, fakeQr));
    }
}
