using FSIT.PayLink.Application.Abstractions.Events;
using FSIT.PayLink.Application.Abstractions.Payments;
using FSIT.PayLink.Application.Abstractions.Tenancy;
using FSIT.PayLink.Domain.Repositories;
using FSIT.PayLink.Infrastructure.Events;
using FSIT.PayLink.Infrastructure.Payments;
using FSIT.PayLink.Infrastructure.Persistence;
using FSIT.PayLink.Infrastructure.Webhooks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace FSIT.PayLink.Infrastructure.Extensions;

public static class InfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,
                                                       IConfiguration configuration)
    {
        services.AddDbContext<PayLinkDbContext>(opt =>
            opt.UseNpgsql(
                configuration.GetConnectionString("Default"),
                npg => npg.MigrationsAssembly(typeof(PayLinkDbContext).Assembly.FullName)
            )
        );

        services.AddScoped<IPaymentRepository, PaymentRepository>();
        services.AddSingleton<IEventPublisher, FakeEventPublisher>();
        services.AddSingleton<IAbacateWebhookVerifier, AbacateWebhookVerifier>();

        services.AddSingleton<IPaymentProvider, FakePaymentProvider>();

        services.AddHttpClient<AbacatePixProvider>(client =>
        {
            client.BaseAddress = new Uri(configuration["Abacate:BaseUrl"]!);
            client.DefaultRequestHeaders.Add("ApiKey", configuration["Abacate:ApiKey"]!);
        })
        .AddTypedClient<IPaymentProvider>((http, sp) =>
            new AbacatePixProvider(
                http,
                sp.GetRequiredService<IPaymentRepository>(),
                sp.GetRequiredService<ITenantContext>(),
                sp.GetRequiredService<IConfiguration>()
            )
        );

        return services;
    }
}
