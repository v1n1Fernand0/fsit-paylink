using FSIT.PayLink.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSIT.PayLink.Infrastructure.Persistence.Mappings;

internal sealed class CustomerExternalMapMap : IEntityTypeConfiguration<CustomerExternalMap>
{
    public void Configure(EntityTypeBuilder<CustomerExternalMap> e)
    {
        e.ToTable("customer_external_map");

        e.HasKey(x => new { x.TenantId, x.Gateway });

        e.Property(x => x.TenantId)
         .HasColumnName("tenant_id")
         .IsRequired();

        e.Property(x => x.Gateway)
         .HasColumnName("gateway")
         .HasMaxLength(32)
         .IsRequired();

        e.Property(x => x.ExternalCustomerId)
         .HasColumnName("external_customer_id")
         .HasMaxLength(64)
         .IsRequired();
    }
}
