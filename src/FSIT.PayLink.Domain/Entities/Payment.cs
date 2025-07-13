using FSIT.PayLink.Domain.Entities;
using FSIT.PayLink.Domain.ValueObjects;

public class Payment
{
    public Guid Id { get; private set; }
    public Money Amount { get; private set; }
    public Cpf PayerCpf { get; private set; }
    public string TenantId { get; private set; } = default!;
    public string ProviderId { get; private set; } = default!;
    public string? QrCodeUrl { get; private set; }
    public PaymentStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public static Payment Create(
        Money amount,
        Cpf cpf,
        string tenantId,
        string providerId,
        string? qr)
    {
        return new Payment
        {
            Id = Guid.NewGuid(),
            Amount = amount,
            PayerCpf = cpf,
            TenantId = tenantId,
            ProviderId = providerId,
            QrCodeUrl = qr,
            Status = PaymentStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void MarkSucceeded() =>
        Status = PaymentStatus.Succeeded;
}
