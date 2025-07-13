using FSIT.PayLink.Domain.Entities;
using FSIT.PayLink.Infrastructure.Persistence.Mappings;
using Microsoft.EntityFrameworkCore;

namespace FSIT.PayLink.Infrastructure.Persistence;

public class PayLinkDbContext : DbContext
{
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<CustomerExternalMap> CustomerExternalMaps => Set<CustomerExternalMap>();

    public PayLinkDbContext(DbContextOptions<PayLinkDbContext> opts) : base(opts) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new PaymentMap());
        builder.ApplyConfiguration(new CustomerExternalMapMap());
    }

}
