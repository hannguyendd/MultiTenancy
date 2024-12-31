using Finbuckle.MultiTenant;
using Finbuckle.MultiTenant.Abstractions;

namespace DatabasePerTenant.Shared;

public class AppTenantInfo : ITenantInfo
{
    public string Id { get; set; } = string.Empty;
    public string Identifier { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string ConnectionString { get; set; } = string.Empty;
}

// public class xxx : MultiTenantContext<AppTenantInfo>
// {
//     public xxx(ITenantInfo tenantInfo) : base(tenantInfo)
//     {
//     }
// }