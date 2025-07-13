
using FSIT.PayLink.Application.Abstractions.Messaging;  
using FSIT.PayLink.Application.Extensions;                
using FSIT.PayLink.Application.Features.Charges.Commands;
using FSIT.PayLink.Application.Features.Charges.Queries;
using FSIT.PayLink.Contracts.Charges;
using FSIT.PayLink.Infrastructure.Extensions;             
using FSIT.PayLink.Infrastructure.Persistence;          
using FSIT.PayLink.Api.Tenancy;                           
using FSIT.PayLink.Application.Abstractions.Tenancy;
using Microsoft.EntityFrameworkCore;
using FSIT.PayLink.Application.Features.Webhooks.AbacatePix;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ITenantContext, HttpTenantContext>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseMiddleware<TenantContextMiddleware>();           

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<PayLinkDbContext>();
    db.Database.Migrate();

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/health", () => Results.Ok("OK"));

app.MapPost("/charges", async (
        ChargeRequest dto,
        IDispatcher dispatcher,
        CancellationToken ct) =>
{
    var cmd = new CreateChargeCommand(dto.Amount, dto.Currency, dto.TenantId);
    var res = await dispatcher.Send(cmd, ct);
    return Results.Ok(res);
});

app.MapGet("/charges/{id:guid}", async (
        Guid id,
        IDispatcher dispatcher,
        CancellationToken ct) =>
{
    var res = await dispatcher.Send(new GetChargeStatusQuery(id), ct);
    return res.Status == "not-found"
        ? Results.NotFound()
        : Results.Ok(res);
});

app.MapPost("/webhook/abacatepay", async (
        HttpRequest req,
        IDispatcher dispatcher,
        CancellationToken ct) =>
{
    using var r = new StreamReader(req.Body);
    string body = await r.ReadToEndAsync(ct);
    var cmd = new AbacateBillingPaidCommand(body, req); 
    return await dispatcher.Send(cmd, ct);
});

app.Run();
