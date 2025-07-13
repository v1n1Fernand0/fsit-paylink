namespace FSIT.PayLink.Contracts.Charges;

/// <summary>DTO devolvido pela API após criar/consultar cobrança.</summary>
public sealed record ChargeResult(
    Guid PaymentId,
    string Status,         
    string? QrCodeUrl      
);
