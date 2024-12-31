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

builder.Services.AddDbContext<ApplicationDbContext>((option) =>
{
    if (EF.IsDesignTime)
    {
        Console.WriteLine("Design time detected");
        var tenants = builder.Configuration.GetSection("Finbuckle:MultiTenant:Stores:ConfigurationStore:Tenants").Get<List<AppTenantInfo>>() ?? [];

        Console.WriteLine(string.Join(Environment.NewLine, tenants.Select(x => x.ConnectionString)));
        option.UseNpgsql(tenants.FirstOrDefault()?.ConnectionString);
    }
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
