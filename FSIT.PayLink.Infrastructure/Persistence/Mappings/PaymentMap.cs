using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSIT.PayLink.Infrastructure.Persistence.Mappings;

internal class PaymentMap : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> e)
    {
        e.ToTable("payments");

        e.HasKey(p => p.Id);

        e.OwnsOne(p => p.Amount, m =>
        {
            m.Property(x => x.Amount)
             .HasColumnName("amount")
             .HasColumnType("numeric(18,2)")
             .IsRequired();

            m.Property(x => x.Currency)
             .HasColumnName("currency")
             .HasMaxLength(3)
             .IsRequired();
        });

        e.Property(p => p.PayerCpf.Value)
        .HasColumnName("payer_cpf")
        .HasMaxLength(11)
        .IsRequired();

        e.Property(p => p.CreatedAt)
         .HasColumnName("created_at")
         .IsRequired();
        e.Property(p => p.TenantId).HasColumnName("tenant_id").IsRequired();
        e.Property(p => p.ProviderId).HasColumnName("provider_id").IsRequired();
        e.Property(p => p.Status).HasColumnName("status").IsRequired();
        e.Property(p => p.QrCodeUrl).HasColumnName("qr_code_url");
    }
}
