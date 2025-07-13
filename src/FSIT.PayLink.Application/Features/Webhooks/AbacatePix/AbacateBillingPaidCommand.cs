using Microsoft.AspNetCore.Http;
using FSIT.PayLink.Application.Abstractions.Messaging;

namespace FSIT.PayLink.Application.Features.Webhooks.AbacatePix;

/// <summary>
/// Command que carrega o corpo bruto do webhook e o HttpRequest
/// para validação do secret.
/// </summary>
public sealed record AbacateBillingPaidCommand(
    string RawBody,
    HttpRequest Http)
    : ICommand<WebhookResult>;
