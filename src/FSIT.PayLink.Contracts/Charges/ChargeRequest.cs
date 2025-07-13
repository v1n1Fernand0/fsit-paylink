namespace FSIT.PayLink.Contracts.Charges;

/// <summary>DTO enviado pelo front-end para criar uma cobrança.</summary>
public sealed record ChargeRequest(
    decimal Amount,
    string Currency,
    string TenantId,
    string Cpf);          
