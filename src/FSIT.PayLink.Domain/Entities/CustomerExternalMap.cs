namespace FSIT.PayLink.Domain.Entities;

/// <summary>
/// Guarda o vínculo entre o Tenant interno e o <c>customerId</c>
/// que o gateway (ex.: AbacatePay) devolveu.
/// </summary>
public class CustomerExternalMap
{
    public Guid TenantId { get; set; }
    public string Gateway { get; set; } = default!;
    public string ExternalCustomerId { get; set; } = default!;
}
