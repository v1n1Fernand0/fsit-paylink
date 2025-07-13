namespace FSIT.PayLink.Contracts.Events.v1;

/// <summary>Evento publicado quando o pagamento liquida.</summary>
public sealed record PaymentSucceeded(
    Guid PaymentId,
    decimal Amount,
    string Currency,
    string TenantId,
    DateTimeOffset SettledAtUtc
);
