using FSIT.PayLink.Application.Abstractions.Events;
using FSIT.PayLink.Application.Abstractions.Messaging;
using FSIT.PayLink.Application.Abstractions.Payments;
using FSIT.PayLink.Contracts.Abacate;
using FSIT.PayLink.Contracts.Events.v1;
using FSIT.PayLink.Domain.Entities;
using FSIT.PayLink.Domain.Repositories;
using System.Text.Json;

namespace FSIT.PayLink.Application.Features.Webhooks.AbacatePix;

public sealed class AbacateBillingPaidHandler
    : IHandler<AbacateBillingPaidCommand, WebhookResult>
{
    private readonly IAbacateWebhookVerifier _verifier;
    private readonly IPaymentRepository _repo;
    private readonly IEventPublisher _publisher;

    public AbacateBillingPaidHandler(
        IAbacateWebhookVerifier verifier,
        IPaymentRepository repo,
        IEventPublisher publisher)
    {
        _verifier = verifier;
        _repo = repo;
        _publisher = publisher;
    }

    public async Task<WebhookResult> Handle(
        AbacateBillingPaidCommand cmd,
        CancellationToken ct)
    {
        if (!_verifier.Verify(cmd.Http))
            return WebhookResult.Unauthorized;

        AbacateBillingPaid payload;
        try
        {
            payload = JsonSerializer.Deserialize<AbacateBillingPaid>(cmd.RawBody)
                      ?? throw new JsonException();
        }
        catch
        {
            return WebhookResult.NotFound;
        }

        var providerId = payload.Data.PixQrCode?.Id;
        if (string.IsNullOrWhiteSpace(providerId))
            return WebhookResult.Ok;

        var payment = await _repo.GetByProviderIdAsync(providerId, ct);
        if (payment is null)
            return WebhookResult.NotFound;

        if (payment.Status != PaymentStatus.Succeeded)
        {
            payment.MarkSucceeded();
            await _repo.SaveAsync(ct);

            var evt = new PaymentSucceeded(
                payment.Id,
                payment.Amount.Amount,
                payment.Amount.Currency,
                payment.TenantId,
                DateTimeOffset.UtcNow);

            await _publisher.PublishAsync(evt, ct);
        }

        return WebhookResult.Ok;
    }
}
