using FSIT.PayLink.Application.Abstractions.Payments;
using FSIT.PayLink.Application.Abstractions.Tenancy;          // ← novo
using FSIT.PayLink.Domain.Repositories;
using FSIT.PayLink.Infrastructure.Payments.Dtos;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;

namespace FSIT.PayLink.Infrastructure.Payments;

/// <summary>Implementação real do <see cref="IPaymentProvider"/> usando a API AbacatePay.</summary>
public sealed class AbacatePixProvider : IPaymentProvider
{
    private readonly HttpClient _http;
    private readonly IPaymentRepository _repo;
    private readonly ITenantContext _tenant;           
    private readonly string _currencyDefault;

    public AbacatePixProvider(HttpClient http,
                              IPaymentRepository repo,
                              ITenantContext tenant,     
                              IConfiguration cfg)
    {
        _http = http;
        _repo = repo;
        _tenant = tenant;
        _currencyDefault = cfg["Abacate:DefaultCurrency"] ?? "BRL";
    }

    public async Task<(string ProviderId, string? QrCodeUrl)>
        CreateChargeAsync(decimal amount, string currency, CancellationToken ct)
    {
        Guid tenantId = _tenant.CurrentTenantId;
        if (tenantId == Guid.Empty)
            throw new InvalidOperationException("TenantId ausente na requisição.");

        string customerId = await EnsureCustomerAsync(tenantId, ct);

        var rq = new CreateQrRequest(
            customerId,
            (int)(amount * 100),                         
            string.IsNullOrWhiteSpace(currency)
                ? _currencyDefault
                : currency.ToUpperInvariant());

        var response = await _http.PostAsJsonAsync("/pix-qrcode", rq, ct);
        response.EnsureSuccessStatusCode();

        var rs = await response.Content.ReadFromJsonAsync<CreateQrResponse>(ct)!;
        return (rs.PixQrCode.Id, rs.PixQrCode.Image);
    }

    private async Task<string> EnsureCustomerAsync(Guid tenantId, CancellationToken ct)
    {
        var map = await _repo.GetCustomerExternalMapAsync(tenantId, "Abacate", ct);
        if (map is not null) return map.ExternalCustomerId;

        var rq = new CreateCustomerRequest($"tenant-{tenantId:N}");
        var rsp = await _http.PostAsJsonAsync("/clientes", rq, ct);
        rsp.EnsureSuccessStatusCode();

        var rs = await rsp.Content.ReadFromJsonAsync<CreateCustomerResponse>(ct)!;
        await _repo.AddCustomerExternalMapAsync(tenantId, "Abacate", rs.Id, ct);
        return rs.Id;
    }
}
