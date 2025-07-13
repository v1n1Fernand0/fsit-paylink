using Microsoft.AspNetCore.Http;
using FSIT.PayLink.Application.Abstractions.Messaging;

namespace FSIT.PayLink.Application.Features.Webhooks.AbacatePix;

/// <summary>Command despachado pelo endpoint /webhook/abacatepay.</summary>
public sealed record AbacateBillingPaidCommand(
    string RawBody,
    HttpRequest Http)
    : ICommand<WebhookResult>;      // ← retorna enum (não IResult)
