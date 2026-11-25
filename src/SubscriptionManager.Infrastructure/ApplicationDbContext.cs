using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SubscriptionManager.Domain.Entities;
using SubscriptionManager.Domain.ValueObjects;

namespace SubscriptionManager.Infrastructure;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Subscription> Subscriptions => Set<Subscription>();
    public DbSet<User> Users => Set<User>();

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder
            .Properties<Currency>()
            .HaveConversion<CurrencyConverter>()
            .HaveMaxLength(3);

        configurationBuilder
            .Properties<Email>()
            .HaveConversion<EmailConverter>()
            .HaveMaxLength(320);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(ApplicationDbContext).Assembly);

        modelBuilder.Entity<Subscription>()
            .HasOne(s => s.User)
            .WithMany(u => u.Subscriptions)
            .HasForeignKey(s => s.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}



