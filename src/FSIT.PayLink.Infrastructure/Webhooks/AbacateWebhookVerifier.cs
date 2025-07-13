using FSIT.PayLink.Application.Abstractions.Payments;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;

namespace FSIT.PayLink.Infrastructure.Webhooks;

/// <summary>Implementa a verificação de ?webhookSecret=.</summary>
public sealed class AbacateWebhookVerifier : IAbacateWebhookVerifier
{
    private readonly string _secret;
    public AbacateWebhookVerifier(IConfiguration cfg) =>
        _secret = cfg["Abacate:WebhookSecret"] ?? string.Empty;

    public bool Verify(HttpRequest req) =>
        req.Query["webhookSecret"] == _secret;
}
