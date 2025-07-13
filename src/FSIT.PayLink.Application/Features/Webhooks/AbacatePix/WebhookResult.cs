namespace FSIT.PayLink.Application.Features.Webhooks.AbacatePix;

/// <summary>
/// Resultado interno que o handler devolve ao endpoint.
/// Mantém Application desacoplada de ASP.NET.
/// </summary>
public enum WebhookResult
{
    Ok,
    Unauthorized,
    NotFound
}
