using Finbuckle.MultiTenant;
using Finbuckle.MultiTenant.Abstractions;
using Finbuckle.MultiTenant.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace SharedDatabase.Infrastructure;

public class SharedDatabaseContext : MultiTenantIdentityDbContext<AppUser>
{
    public SharedDatabaseContext(IMultiTenantContextAccessor multiTenantContextAccessor) : base(multiTenantContextAccessor) { }

    public SharedDatabaseContext(IMultiTenantContextAccessor multiTenantContextAccessor, DbContextOptions<SharedDatabaseContext> options) :
        base(multiTenantContextAccessor, options)
    { }

    public DbSet<GoalTemplate> GoalTemplates => Set<GoalTemplate>();

    public DbSet<Goal> Goals => Set<Goal>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<GoalTemplate>().HasKey(x => x.Id);
        modelBuilder.Entity<Goal>().HasKey(x => x.Id);
        modelBuilder.Entity<Goal>().HasOne(x => x.GoalTemplate).WithMany(x => x.Goals).HasForeignKey(x => x.GoalTemplateId);

        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<GoalTemplate>().IsMultiTenant();
        modelBuilder.Entity<Goal>().IsMultiTenant();
    }

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

public class AppUser : IdentityUser
{
    public string FullName { get; set; } = string.Empty;
}