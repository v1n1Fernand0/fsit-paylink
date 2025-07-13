namespace FSIT.PayLink.Domain.Events;

/// <summary>Domain-event emitido quando o gateway confirma o pagamento.</summary>
public sealed record PaymentSucceeded(Guid PaymentId);
