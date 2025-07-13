using Microsoft.AspNetCore.Http;

namespace FSIT.PayLink.Application.Abstractions.Payments;

/// <summary>Valida o segredo passado como query-string no webhook.</summary>
public interface IAbacateWebhookVerifier
{
    bool Verify(HttpRequest request);
}
