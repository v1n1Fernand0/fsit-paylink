namespace FSIT.PayLink.Contracts.Events.v1;

/// <summary>
/// Evento publicado quando o pagamento é recusado ou expira.
/// </summary>
public sealed record PaymentFailed(
    Guid PaymentId,
    decimal Amount,
    string Currency,
    string TenantId,
    DateTimeOffset FailedAtUtc,
    string? Reason         
);
