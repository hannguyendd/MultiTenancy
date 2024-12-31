using Finbuckle.MultiTenant;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using SharedDatabase.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddMultiTenant<TenantInfo>()
    .WithHeaderStrategy("x-tenant-id")
    .WithConfigurationStore();
builder.Services.AddDbContext<SharedDatabaseContext>(option =>
{
    var connectionString = builder.Configuration.GetSection("Finbuckle:MultiTenant:Stores:ConfigurationStore:Defaults:ConnectionString").Value;
    option.UseNpgsql(connectionString);
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapScalarApiReference();
    app.MapOpenApi();
}

app.UseMultiTenant();
app.UseHttpsRedirection();
app.MapControllers();

app.Run();
