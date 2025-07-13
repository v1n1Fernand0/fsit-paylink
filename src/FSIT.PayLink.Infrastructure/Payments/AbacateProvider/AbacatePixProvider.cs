using FSIT.PayLink.Application.Abstractions.Payments;
using FSIT.PayLink.Application.Abstractions.Tenancy;
using FSIT.PayLink.Domain.Repositories;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace FSIT.PayLink.Infrastructure.Payments;

public sealed class AbacatePixProvider : IPaymentProvider
{
    private readonly HttpClient _http;
    private readonly IPaymentRepository _repo;
    private readonly ITenantContext _tenant;
    private readonly string _key;
    private readonly string _currencyDefault;

    public AbacatePixProvider(HttpClient http,
                              IPaymentRepository repo,
                              ITenantContext tenant,
                              IConfiguration cfg)
    {
        _http = http;
        _repo = repo;
        _tenant = tenant;
        _key = cfg["Abacate:ApiKey"]!;
        _currencyDefault = cfg["Abacate:DefaultCurrency"] ?? "BRL";

        _http.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _key);
    }

    public async Task<(string ProviderId, string? QrCodeUrl)> CreateChargeAsync(
        decimal amount,
        string currency,
        string cpf,      
        CancellationToken ct)
    {
        var tenantId = _tenant.CurrentTenantId;
        if (tenantId == Guid.Empty)
            throw new InvalidOperationException("TenantId ausente na requisição.");

        var customerId = await EnsureCustomerAsync(tenantId, cpf, ct);

        var qrRequest = new
        {
            amount = (int)(amount * 100),
            expiresIn = 3600,
            description = $"Pagamento tenant {tenantId:N}",
            customer = new
            {
                id = customerId,
                name = $"tenant-{tenantId:N}",
                taxId = cpf,
                email = "test.user@example.com",
                cellphone = "+5511999998888"
            }
        };



        var qrResponse = await _http.PostAsJsonAsync(
            "/v1/pixQrCode/create", qrRequest, ct);

        if (!qrResponse.IsSuccessStatusCode)
        {
            var errorBody = await qrResponse.Content.ReadAsStringAsync(ct);
            throw new HttpRequestException(
                $"Erro {qrResponse.StatusCode} ao criar qrcode: {errorBody}");
        }

        var qrPayload = await qrResponse.Content
            .ReadFromJsonAsync<JsonResponse<PixQrCodeData>>(cancellationToken: ct)!;

        return (qrPayload.Data.Id, qrPayload.Data.BrCodeBase64);
    }

    private async Task<string> EnsureCustomerAsync(
        Guid tenantId,
        string cpf,        
        CancellationToken ct)
    {
        var map = await _repo.GetCustomerExternalMapAsync(tenantId, "Abacate", ct);
        if (map is not null)
            return map.ExternalCustomerId;

        var custRequest = new
        {
            name = $"tenant-{tenantId:N}",
            taxId = cpf,                        
            email = "no-reply@paylink.local",
            cellphone = "+550000000000"
        };

        var custResponse = await _http.PostAsJsonAsync(
            "/v1/customer/create", custRequest, ct);

        if (!custResponse.IsSuccessStatusCode)
        {
            var errorBody = await custResponse.Content.ReadAsStringAsync(ct);
            throw new HttpRequestException(
                $"Erro {custResponse.StatusCode} ao criar cliente: {errorBody}");
        }


        var custPayload = await custResponse.Content
            .ReadFromJsonAsync<JsonResponse<CustomerData>>(cancellationToken: ct)!;

        await _repo.AddCustomerExternalMapAsync(
            tenantId, "Abacate", custPayload.Data.Id, ct);

        return custPayload.Data.Id;
    }

    private record JsonResponse<T>(T Data);
    private record CustomerData(string Id);
    private record PixQrCodeData(string Id, string BrCodeBase64);
}
