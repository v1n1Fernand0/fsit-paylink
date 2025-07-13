using static FSIT.PayLink.Infrastructure.Payments.Dtos.CreateQrResponse;

namespace FSIT.PayLink.Infrastructure.Payments.Dtos;

/// <summary>Resposta de POST /pix-qrcode.</summary>
internal sealed record CreateQrResponse(CreateQrPix PixQrCode)
{
    internal sealed record CreateQrPix(
        string Id,
        string Image,
        string Status);
}
