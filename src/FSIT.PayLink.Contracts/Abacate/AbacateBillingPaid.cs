using static FSIT.PayLink.Contracts.Abacate.AbacateBillingPaid;

namespace FSIT.PayLink.Contracts.Abacate;

/// <summary>Payload que o AbacatePay envia no webhook billing.paid.</summary>
public sealed record AbacateBillingPaid(
    string Event,   
    bool DevMode,
    BillingData Data)
{
    public sealed record BillingData(
        PaymentInfo Payment,
        PixQrCode PixQrCode);

    public sealed record PaymentInfo(
        int Amount,
        int Fee,
        string Method);

    public sealed record PixQrCode(
        string Id,
        string Status);
}
