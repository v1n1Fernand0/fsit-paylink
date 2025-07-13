using FSIT.PayLink.Application.Abstractions.Messaging;
using FSIT.PayLink.Contracts.Charges;

namespace FSIT.PayLink.Application.Features.Charges.Commands;

public record CreateChargeCommand(decimal Amount,
                                  string Currency,
                                  string TenantId)
       : ICommand<ChargeResult>;
