using FSIT.PayLink.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FSIT.PayLink.Infrastructure.Persistence;

public class PayLinkDbContext : DbContext
{
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<CustomerExternalMap> CustomerExternalMaps => Set<CustomerExternalMap>();

    public PayLinkDbContext(DbContextOptions<PayLinkDbContext> opts) : base(opts) { }

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.ApplyConfiguration(new Mappings.PaymentMap());
        b.ApplyConfiguration(new Mappings.CustomerExternalMapMap());
    }
}
