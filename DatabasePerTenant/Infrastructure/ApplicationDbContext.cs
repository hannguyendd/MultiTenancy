using DatabasePerTenant.Shared;
using Finbuckle.MultiTenant.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace DatabasePerTenant.Infrastructure;

public class ApplicationDbContext : DbContext
{
    private AppTenantInfo TenantInfo { get; set; }

    public ApplicationDbContext(IMultiTenantContextAccessor<AppTenantInfo> multiTenantContextAccessor, DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        // get the current tenant info at the time of construction
        TenantInfo = multiTenantContextAccessor.MultiTenantContext?.TenantInfo ?? new();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!EF.IsDesignTime)
        {
            optionsBuilder.UseNpgsql(TenantInfo.ConnectionString);
        }
        else
        {
            base.OnConfiguring(optionsBuilder);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<GoalTemplate>().HasKey(x => x.Id);
        modelBuilder.Entity<Goal>().HasKey(x => x.Id);
        modelBuilder.Entity<Goal>().HasOne(x => x.GoalTemplate).WithMany(x => x.Goals).HasForeignKey(x => x.GoalTemplateId);
    }

    public DbSet<GoalTemplate> GoalTemplates => Set<GoalTemplate>();

    public DbSet<Goal> Goals => Set<Goal>();
}

public class GoalTemplate
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public ICollection<Goal> Goals { get; } = [];
}

public class Goal
{
    public Guid Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public Guid GoalTemplateId { get; set; }
    public GoalTemplate GoalTemplate { get; set; } = null!;
}