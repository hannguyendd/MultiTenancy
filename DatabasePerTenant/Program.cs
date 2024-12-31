using DatabasePerTenant.Infrastructure;
using DatabasePerTenant.Shared;
using Finbuckle.MultiTenant;
using Finbuckle.MultiTenant.Abstractions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddMultiTenant<AppTenantInfo>()
    .WithHeaderStrategy()
    .WithConfigurationStore();

builder.Services.AddDbContext<ApplicationDbContext>((serviceProvider, dbContextOptionsBuilder) =>
{
    var connectionString = Array.IndexOf(args, "--connection-string") >= 0
        ? args[Array.IndexOf(args, "--connection-string") + 1]
        : serviceProvider.GetRequiredService<IMultiTenantContextAccessor<AppTenantInfo>>()
            .MultiTenantContext?.TenantInfo?.ConnectionString ?? string.Empty;

    dbContextOptionsBuilder.UseNpgsql(connectionString);
});


builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseMultiTenant();

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
