using FSIT.PayLink.Application.Abstractions.Messaging;
using FSIT.PayLink.Contracts.Charges;
using FSIT.PayLink.Domain.Repositories;

namespace FSIT.PayLink.Application.Features.Charges.Queries;

public class GetChargeStatusHandler
    : IHandler<GetChargeStatusQuery, ChargeResult>
{
    private readonly IPaymentRepository _repo;
    public GetChargeStatusHandler(IPaymentRepository repo) => _repo = repo;

    public async Task<ChargeResult> Handle(GetChargeStatusQuery q,
                                           CancellationToken ct)
    {
        var p = await _repo.GetAsync(q.PaymentId, ct);

        return p is null
            ? new ChargeResult(q.PaymentId, "not-found", null)
            : new ChargeResult(p.Id, p.Status.ToString(), p.QrCodeUrl);
    }
}
