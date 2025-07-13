using FSIT.PayLink.Application.Abstractions.Tenancy;

namespace FSIT.PayLink.Api.Tenancy;

public sealed class HttpTenantContext : ITenantContext
{
    private readonly IHttpContextAccessor _acc;
    public HttpTenantContext(IHttpContextAccessor acc) => _acc = acc;

    public Guid CurrentTenantId =>
        _acc.HttpContext?.Items.TryGetValue("TenantId", out var v) == true && v is Guid g
            ? g
            : Guid.Empty;
}
