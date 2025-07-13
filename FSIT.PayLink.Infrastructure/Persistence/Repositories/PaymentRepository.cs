using FSIT.PayLink.Domain.Entities;
using FSIT.PayLink.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FSIT.PayLink.Infrastructure.Persistence;

public class PaymentRepository : IPaymentRepository
{
    private readonly PayLinkDbContext _db;
    public PaymentRepository(PayLinkDbContext db) => _db = db;

    /* -----------------------------------------------------------------------
       Pagamentos
    ----------------------------------------------------------------------- */

    public async Task AddAsync(Payment entity, CancellationToken ct)
    {
        _db.Payments.Add(entity);
        await _db.SaveChangesAsync(ct);
    }

    public Task<Payment?> GetAsync(Guid id, CancellationToken ct) =>
        _db.Payments
           .AsNoTracking()
           .FirstOrDefaultAsync(p => p.Id == id, ct);

    public Task<Payment?> GetByProviderIdAsync(string providerId, CancellationToken ct) =>
        _db.Payments
           .FirstOrDefaultAsync(p => p.ProviderId == providerId, ct);

    public Task SaveAsync(CancellationToken ct) => _db.SaveChangesAsync(ct);

    /* -----------------------------------------------------------------------
       Mapeamento de Cliente Externo (Abacate)
    ----------------------------------------------------------------------- */

    public Task<CustomerExternalMap?> GetCustomerExternalMapAsync(
        Guid tenantId, string gateway, CancellationToken ct) =>
        _db.CustomerExternalMaps                       // DbSet no DbContext
           .AsNoTracking()
           .FirstOrDefaultAsync(m =>
                m.TenantId == tenantId && m.Gateway == gateway, ct);

    public async Task AddCustomerExternalMapAsync(
        Guid tenantId, string gateway, string extId, CancellationToken ct)
    {
        _db.CustomerExternalMaps.Add(new CustomerExternalMap
        {
            TenantId = tenantId,
            Gateway = gateway,
            ExternalCustomerId = extId
        });
        await _db.SaveChangesAsync(ct);
    }
}
