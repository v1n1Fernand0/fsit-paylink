using FSIT.PayLink.Domain.Events;
using FSIT.PayLink.Domain.ValueObjects;

namespace FSIT.PayLink.Domain.Entities;

/// <summary>Aggregate-root que representa uma cobrança.</summary>
public class Payment
{
    public Guid Id { get; private set; }
    public string TenantId { get; private set; } = default!;
    public Money Amount { get; private set; } = default!;
    public string ProviderId { get; private set; } = default!;
    public PaymentStatus Status { get; private set; } = PaymentStatus.Pending;
    public string? QrCodeUrl { get; private set; }

    private Payment() { }

    public static Payment Create(Money amount,
                                 string tenantId,
                                 string providerId,
                                 string? qrCodeUrl)
        => new()
        {
            Id = Guid.NewGuid(),
            Amount = amount,
            TenantId = tenantId,
            ProviderId = providerId,
            QrCodeUrl = qrCodeUrl
        };

    public PaymentSucceeded MarkSucceeded()
    {
        if (Status != PaymentStatus.Succeeded)
            Status = PaymentStatus.Succeeded;

        return new PaymentSucceeded(Id);
    }

    public void MarkFailed() => Status = PaymentStatus.Failed;
}
