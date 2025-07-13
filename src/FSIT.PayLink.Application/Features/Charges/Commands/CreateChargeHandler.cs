using FSIT.PayLink.Application.Abstractions.Events;
using FSIT.PayLink.Application.Abstractions.Messaging;
using FSIT.PayLink.Application.Abstractions.Payments;
using FSIT.PayLink.Application.Features.Charges.Commands;
using FSIT.PayLink.Contracts.Charges;
using FSIT.PayLink.Contracts.Events.v1;
using FSIT.PayLink.Domain.Entities;
using FSIT.PayLink.Domain.Repositories;
using FSIT.PayLink.Domain.ValueObjects;

public class CreateChargeHandler
    : IHandler<CreateChargeCommand, ChargeResult>
{
    private readonly IPaymentProvider _provider;
    private readonly IPaymentRepository _repo;
    private readonly IEventPublisher _publisher;

    public CreateChargeHandler(IPaymentProvider provider,
                               IPaymentRepository repo,
                               IEventPublisher publisher)
        => (_provider, _repo, _publisher) = (provider, repo, publisher);

    public async Task<ChargeResult> Handle(CreateChargeCommand c, CancellationToken ct)
    {
        var gtw = await _provider.CreateChargeAsync(c.Amount, c.Currency, ct);

        var payment = Payment.Create(
            Money.Of(c.Amount, c.Currency),
            c.TenantId, gtw.ProviderId, gtw.QrCodeUrl);

        await _repo.AddAsync(payment, ct);

        var evt = new PaymentSucceeded(
            payment.Id,
            payment.Amount.Amount,
            payment.Amount.Currency,
            payment.TenantId,
            DateTimeOffset.UtcNow);

        await _publisher.PublishAsync(evt, ct);

        return new ChargeResult(payment.Id, payment.Status.ToString(), payment.QrCodeUrl);
    }
}
