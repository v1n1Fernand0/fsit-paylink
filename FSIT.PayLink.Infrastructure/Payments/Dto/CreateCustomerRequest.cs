namespace FSIT.PayLink.Infrastructure.Payments.Dtos;

/// <summary>Payload enviado a POST /clientes.</summary>
internal sealed record CreateCustomerRequest(string Name);
