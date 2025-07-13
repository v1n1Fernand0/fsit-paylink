using FSIT.PayLink.Application.Abstractions.Messaging;
using FSIT.PayLink.Contracts.Charges;

namespace FSIT.PayLink.Application.Features.Charges.Queries;

public record GetChargeStatusQuery(Guid PaymentId)
       : IQuery<ChargeResult>;
