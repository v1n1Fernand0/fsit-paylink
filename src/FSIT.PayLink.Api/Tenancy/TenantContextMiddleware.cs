namespace FSIT.PayLink.Api.Tenancy;

public sealed class TenantContextMiddleware
{
    private readonly RequestDelegate _next;
    private const string HeaderName = "x-tenant-id";

    public TenantContextMiddleware(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(HttpContext ctx)
    {
        if (ctx.Request.Headers.TryGetValue(HeaderName, out var raw) &&
            Guid.TryParse(raw, out var id))
        {
            ctx.Items["TenantId"] = id;
        }

        await _next(ctx);
    }
}
