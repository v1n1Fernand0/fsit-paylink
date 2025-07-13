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
        if (await _repo.HasPaidInWindowAsync(c.Cpf, c.TenantId, 30, ct))
            return new ChargeResult(Guid.Empty, "duplicate-within-window", null);

        var (provId, qr) = await _provider.CreateChargeAsync(
            c.Amount, c.Currency, ct);

        var payment = Payment.Create(
            Money.Of(c.Amount, c.Currency),
            Cpf.Of(c.Cpf),
            c.TenantId,
            provId,
            qr);

        await _repo.AddAsync(payment, ct);
        return new ChargeResult(payment.Id, payment.Status.ToString(), payment.QrCodeUrl);
    }
}
