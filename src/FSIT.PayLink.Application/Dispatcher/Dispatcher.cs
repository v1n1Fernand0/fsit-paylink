using FSIT.PayLink.Application.Abstractions;
using FSIT.PayLink.Application.Abstractions.Messaging;
using Microsoft.Extensions.DependencyInjection;

namespace FSIT.PayLink.Application.Messaging;

public sealed class Dispatcher : IDispatcher
{
    private readonly IServiceProvider _sp;
    public Dispatcher(IServiceProvider sp) => _sp = sp;

    public Task<TResponse> Send<TResponse>(IRequest<TResponse> req,
                                           CancellationToken ct = default)
    {
        var handlerType = typeof(IHandler<,>)
            .MakeGenericType(req.GetType(), typeof(TResponse));

        dynamic handler = _sp.GetRequiredService(handlerType);
        return handler.Handle((dynamic)req, ct);
    }
}
