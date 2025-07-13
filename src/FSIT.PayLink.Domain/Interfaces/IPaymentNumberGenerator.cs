namespace FSIT.PayLink.Domain.Services;

/// <summary>Contrato para gerar um número humano-legível da cobrança.</summary>
public interface IPaymentNumberGenerator
{
    /// <returns>Ex.: "PX-2025-000123"</returns>
    string Next(string tenantId);
}
