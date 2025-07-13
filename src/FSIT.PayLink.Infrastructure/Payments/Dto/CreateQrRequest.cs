namespace FSIT.PayLink.Infrastructure.Payments.Dtos;

/// <summary>Payload enviado a POST /pix-qrcode.</summary>
internal sealed record CreateQrRequest(
    string CustomerId,
    int Amount,         
    string Currency);
