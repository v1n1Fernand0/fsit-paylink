using System.Reflection;
using FSIT.PayLink.Application.Abstractions.Messaging;
using FSIT.PayLink.Application.Messaging;
using Microsoft.Extensions.DependencyInjection;

namespace FSIT.PayLink.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection s)
    {
        s.AddScoped<IDispatcher, Dispatcher>();

        s.Scan(scan => scan
            .FromAssemblies(Assembly.GetExecutingAssembly())  
            .AddClasses(c => c.AssignableTo(typeof(IHandler<,>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        return s;
    }
}
