using FSIT.PayLink.Domain.Entities;

namespace FSIT.PayLink.Domain.Repositories;

/// <summary>
/// Operações de persistência relacionadas a pagamentos e
/// ao vínculo de clientes externos com gateways de pagamento.
/// </summary>
public interface IPaymentRepository
{
    /// <summary>Insere um novo pagamento.</summary>
    Task AddAsync(Payment entity, CancellationToken ct);

    /// <summary>Obtém um pagamento pelo Id interno.</summary>
    Task<Payment?> GetAsync(Guid id, CancellationToken ct);

    /// <summary>Obtém um pagamento pelo Id retornado pelo gateway (ProviderId).</summary>
    Task<Payment?> GetByProviderIdAsync(string providerId, CancellationToken ct);

    /// <summary>Persiste alterações pendentes (update/delete).</summary>
    Task SaveAsync(CancellationToken ct);

    /// <summary>
    /// Retorna o <c>customerId</c> correspondente a um tenant + gateway,
    /// ou <c>null</c> se ainda não houver.
    /// </summary>
    Task<CustomerExternalMap?> GetCustomerExternalMapAsync(
        Guid tenantId,
        string gateway,
        CancellationToken ct);

    /// <summary>
    /// Armazena o vínculo entre <c>tenantId</c> e o <c>customerId</c>
    /// gerado pelo gateway.
    /// </summary>
    Task AddCustomerExternalMapAsync(
        Guid tenantId,
        string gateway,
        string extId,
        CancellationToken ct);

    Task<bool> HasPaidInWindowAsync(string cpf, string tenantId, int days, CancellationToken ct);

}
