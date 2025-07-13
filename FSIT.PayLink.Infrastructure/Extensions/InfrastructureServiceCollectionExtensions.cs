using FSIT.PayLink.Application.Abstractions.Events;
using FSIT.PayLink.Application.Abstractions.Payments;
using FSIT.PayLink.Domain.Repositories;
using FSIT.PayLink.Infrastructure.Events;
using FSIT.PayLink.Infrastructure.Payments;
using FSIT.PayLink.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FSIT.PayLink.Infrastructure.Extensions;

public static class InfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection s,
                                                       IConfiguration cfg)
    {
        s.AddDbContext<PayLinkDbContext>(opt =>
            opt.UseNpgsql(cfg.GetConnectionString("Default"),
                          npg => npg.MigrationsAssembly(typeof(PayLinkDbContext).Assembly.FullName)));

        s.AddScoped<IPaymentRepository, PaymentRepository>();
        s.AddSingleton<IPaymentProvider, FakePaymentProvider>();
        s.AddSingleton<IEventPublisher, FakeEventPublisher>();

        return s;
    }
}
