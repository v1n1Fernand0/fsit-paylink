using FSIT.PayLink.Application.Abstractions.Events;
using FSIT.PayLink.Application.Abstractions.Payments;
using FSIT.PayLink.Domain.Repositories;
using FSIT.PayLink.Infrastructure.Events;
using FSIT.PayLink.Infrastructure.Payments;
using FSIT.PayLink.Infrastructure.Persistence;
using FSIT.PayLink.Infrastructure.Webhooks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FSIT.PayLink.Infrastructure.Extensions;

public static class InfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection service,
                                                       IConfiguration configuration)
    {
        service.AddDbContext<PayLinkDbContext>(opt =>
            opt.UseNpgsql(configuration.GetConnectionString("Default"),
                          npg => npg.MigrationsAssembly(typeof(PayLinkDbContext).Assembly.FullName)));

        service.AddScoped<IPaymentRepository, PaymentRepository>();
        service.AddSingleton<IPaymentProvider, FakePaymentProvider>();
        service.AddSingleton<IEventPublisher, FakeEventPublisher>();
        service.AddSingleton<IAbacateWebhookVerifier, AbacateWebhookVerifier>();



        return service;
    }
}
