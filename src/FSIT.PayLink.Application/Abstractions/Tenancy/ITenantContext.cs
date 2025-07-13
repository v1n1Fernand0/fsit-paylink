namespace FSIT.PayLink.Application.Abstractions.Tenancy;

/// <summary>Fornece o Id do tenant da requisição atual.</summary>
public interface ITenantContext
{
    Guid CurrentTenantId { get; }
}
