namespace FSIT.PayLink.Application.Abstractions.Messaging;

public interface IDispatcher
{
    Task<TResponse> Send<TResponse>(IRequest<TResponse> request,
                                    CancellationToken ct = default);
}
